```javascript
// File: ./features/resource/resource.model
const mongoose = require('mongoose');
const { Schema } = mongoose;
// ========== ResourceType Enum ==========
const ResourceType = Object.freeze({
  TV: 'tv',
  COMPUTER: 'computer',
  SMART_BOARD: 'smart_board',
  PROJECTOR: 'projector',
  FURNITURE: 'furniture',
  OTHER: 'other'
});
// ========== ResourceStatus Enum ==========
const ResourceStatus = Object.freeze({
  AVAILABLE: 'available',
  IN_USE: 'in_use',
  MAINTENANCE: 'maintenance',
  RETIRED: 'retired'
});
// ========== Resource Schema ==========
const resourceSchema = new Schema({
  name: {
    type: String,
    required: true,
    trim: true,
    // frequently queried
  },
  type: {
    type: String,
    required: true,
    enum: Object.values(ResourceType),
  },
  quantity: {
    type: Number,
    required: true,
    min: [1, 'Quantity must be at least 1']
  },
  location: {
    // e.g., Classroom, Lab, Office
    type: String,
    trim: true,
  },
  assignedTo: {
    // optional reference to a User or Department
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
  },
  purchaseDate: {
    type: Date,
  },
  warrantyExpiry: {
    type: Date,
    validate: {
      validator: function (v) {
        return !v || v > this.purchaseDate;
      },
      message: 'warrantyExpiry must be after purchaseDate'
    }
  },
  status: {
    type: String,
    required: true,
    enum: Object.values(ResourceStatus),
    default: ResourceStatus.AVAILABLE,
  },
  notes: {
    type: String,
    trim: true
  },
  isDeleted: {
    type: Boolean,
    default: false,
  },
  user: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
    required: true
  }
}, {
  timestamps: true
});
// attach enums to statics
resourceSchema.statics.ResourceType = ResourceType;
resourceSchema.statics.ResourceStatus = ResourceStatus;
module.exports = mongoose.model('Resource', resourceSchema);

```
