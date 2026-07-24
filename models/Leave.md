```javascript
// File: ./features/leave/leave.model
const mongoose = require('mongoose');
const { Schema } = mongoose;
// LeaveStatus enum
const LeaveStatus = Object.freeze({
  DRAFT: 'draft',
  PENDING: 'pending',
  APPROVED: 'approved',
  REJECTED: 'rejected'
});
// Validator to ensure endDate ≥ startDate
function validateLeaveDates() {
  if (!this) return true;
  if (!this.startDate || !this.endDate) return true;
  return this.endDate >= this.startDate;
}
const leaveSchema = new Schema({
  student: {
    type: Schema.Types.ObjectId,
    ref: 'Student',
    resolveBy: 'username',
  },
  teacher: {
    type: Schema.Types.ObjectId,
    ref: 'Teacher',
    resolveBy: 'username',
  },
  employee: {
    type: Schema.Types.ObjectId,
    ref: 'Employee',
    resolveBy: 'username',
  },
  userType: {
    type: String,
    enum: ['Student', 'Teacher', 'Employee'],
    required: true
  },
  startDate: {
    type: Date,
    required: true,
  },
  endDate: {
    type: Date,
    required: true,
    validate: {
      validator: validateLeaveDates,
      message: (props) => {
        if (!this) return true;
        if (!this.startDate) return true;
        return `endDate (${props.value.toISOString().slice(0, 10)}) must be on or after startDate (${this.startDate.toISOString().slice(0, 10)})}`
      }
    }
  },
  type: {
    type: String,
    required: true,
  },
  reason: {
    type: String,
    trim: true
  },
  comments: [{
    message: {
      type: String,
      trim: true
    },
    author: {
      type: Schema.Types.ObjectId,
      ref: 'User', resolveBy: "username",
    },
    createdAt: {
      type: Date,
      default: Date.now
    }
  }],
  status: {
    type: String,
    required: true,
    enum: Object.values(LeaveStatus),
    default: LeaveStatus.DRAFT,
  },
  requestedBy: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
  },
  approvedBy: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
  },
  rejectedBy: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
  },
  user: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
    required: true
  },
  isDeleted: {
    type: Boolean,
    default: false,
  }
}, {
  timestamps: true
});
module.exports = mongoose.model('Leave', leaveSchema);
```
