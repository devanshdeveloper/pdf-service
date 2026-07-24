```javascript
// File: ./features/assignment/assignment.model
// ================== Assignment Model ==================
const mongoose = require("mongoose");
const { Schema, model } = require("mongoose");
// Status enum for assignment lifecycle
const AssignmentStatus = Object.freeze({
    PENDING: 'pending',
    SUBMITTED: 'submitted',
    GRADED: 'graded'
});
const assignmentSchema = new Schema({
    title: {
        type: String,
        required: true,
        trim: true,
    },
    description: {
        type: String,
        trim: true
    },
    assignedDate: {
        type: Date,
        required: true,
    },
    dueDate: {
        type: Date,
        required: true,
        validate: {
            validator: function (v) {
                if (!this) return true;
                if (!this.assignedDate) return true;
                return v > this.assignedDate;
            },
            message: 'dueDate must be after assignedDate'
        }
    },
    publishLevel: {
        type: String,
        enum: ['course', 'department', 'classroom', 'specific'],
    },
    // Holds IDs depending on level:
    courses: [{ type: Schema.Types.ObjectId, ref: 'Course', resolveBy: 'title' }],
    departments: [{ type: Schema.Types.ObjectId, ref: 'Department', resolveBy: 'name' }],
    classrooms: [{ type: Schema.Types.ObjectId, ref: 'Classroom', resolveBy: 'name' }],
    students: [{ type: Schema.Types.ObjectId, ref: 'Student', resolveBy: 'username' }],
    status: {
        type: String,
        enum: Object.values(AssignmentStatus),
        default: AssignmentStatus.PENDING,
    },
    attachments: [{
        type: String,
        trim: true
    }],
    isDeleted: {
        type: Boolean,
        default: false,
    },
    createdBy: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true
    }
}, {
    timestamps: true
});
// attach enum to statics
assignmentSchema.statics.AssignmentStatus = AssignmentStatus;
module.exports = model('Assignment', assignmentSchema);
```
