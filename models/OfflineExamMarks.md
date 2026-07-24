```javascript
// File: ./features/offline-exam-marks/offline-exam-marks.model
const mongoose = require("mongoose");
const offlineExamMarksSchema = new mongoose.Schema({
    subject: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Subject", resolveBy: "name",
        required: true
    },
    student: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Student",
        required: true
    },
    marks: {
        type: Number,
        required: true,
        default: 0,
        min: 0,
    },
    exam: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "OfflineExam",
        required: true,
    },
    updatedBy: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true,
    },
    isDeleted: {
        type: Boolean,
        default: false,
    },
}, { timestamps: true });
module.exports = mongoose.model("OfflineExamMarks", offlineExamMarksSchema);
```
