```javascript
// File: ./features/course/course.model
const { Schema, model } = require('mongoose');
const CourseStatus = Object.freeze({
    DRAFT: 'draft',
    ACTIVE: 'active',
    ARCHIVED: 'archived'
});
// ========== Course Model ==========
const courseSchema = new Schema({
    title: {
        type: String,
        required: true,
        trim: true,
    },
    code: {
        type: String,
        required: true,
        uppercase: true,
        unique: true,
    },
    department: {
        type: Schema.Types.ObjectId,
        ref: 'Department', resolveBy: "name",
        required: true,
    },
    stream: {
        type: Schema.Types.ObjectId,
        ref: 'Stream', resolveBy: "name",
    },
    subjects: [{
        type: Schema.Types.ObjectId,
        ref: 'Subject', resolveBy: "name",
    }],
    status: {
        type: String,
        enum: Object.values(CourseStatus),
        default: CourseStatus.DRAFT
    },
    isDeleted: {
        type: Boolean,
        default: false,
    },
    user: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true,
    }
}, {
    timestamps: true
});
module.exports = model('Course', courseSchema);
```
