```javascript
// File: ./features/assignment-submission/assignment-submission.model
// ================== AssignmentSubmission Model ==================
const mongoose = require("mongoose");
const { Schema, model } = require("mongoose");
// Status enum for assignment lifecycle
const AssignmentSubmissionStatus = Object.freeze({
    PENDING: 'pending',
    SUBMITTED: 'submitted',
    GRADED: 'graded'
});
const assignmentSubmissionSchema = new Schema({
    title: {
        type: String,
        required: true,
        trim: true,
    },
    description: {
        type: String,
        trim: true
    },
    status: {
        type: String,
        enum: Object.values(AssignmentSubmissionStatus),
        default: AssignmentSubmissionStatus.PENDING,
    },
    attachments: [{
        type: String,
        trim: true
    }],
    isDeleted: {
        type: Boolean,
        default: false,
    },
    assignment: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Assignment", resolveBy: "title",
        required: true
    },
    student: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Student", resolveBy: "username",
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
assignmentSubmissionSchema.statics.AssignmentSubmissionStatus = AssignmentSubmissionStatus;
module.exports = model('AssignmentSubmission', assignmentSubmissionSchema);
```
