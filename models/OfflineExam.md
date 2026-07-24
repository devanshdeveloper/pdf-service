```javascript
// File: ./features/offline-exam/offline-exam.model
const mongoose = require("mongoose");
const offlineExamSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true,
        trim: true,
    },
    description: {
        type: String,
        trim: true,
    },
    classroom: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Classroom",
        required: true,
    },
    subjects: [{
        type: mongoose.Schema.Types.ObjectId,
        ref: "Subject", resolveBy: "name"
    }],
    exam: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "OfflineExam",
    },
    maxMarks: {
        type: Number,
        required: true,
        min: 0,
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
module.exports = mongoose.model("OfflineExam", offlineExamSchema);
```
