```javascript
// File: ./features/test/test.model
const { Schema, model } = require('mongoose');
const testSchema = new Schema({
  title: {
    type: String,
    required: true,
    trim: true
  },
  description: {
    type: String,
    trim: true
  },
  subject: {
    type: Schema.Types.ObjectId,
    ref: 'Subject',
  },
  duration: {
    type: Number,
    required: true,
    default: 0
  },
  fields: [{
    label: {
      type: String,
      required: true
    },
    type: {
      type: String,
      required: true,
    },
    required: {
      type: Boolean,
      default: true
    },
    correctAnswer: {
      type: String,
    },
    marks: {
      type: Number,
    },
    negativeMarks: {
      type: Number,
      default: 0
    },
    props: [
      {
        name: {
          type: String,
          required: true,
        },
        value: {
          type: Schema.Types.Mixed,
        },
      },
    ],
    validators: [
      {
        name: {
          type: String,
          required: true,
        },
        value: {
          type: Schema.Types.Mixed,
        },
      },
    ],
  }],
  theme: {
    type: String,
    default: 'default'
  },
  maxSubmitCount: {
    type: Number,
    default: 1
  },
  datePublished: { type: Date },
  publishLevel: {
    type: String,
    enum: ['course', 'department', 'classroom', 'specific'],
  },
  courses: [{ type: Schema.Types.ObjectId, ref: 'Course' }],
  departments: [{ type: Schema.Types.ObjectId, ref: 'Department' }],
  classrooms: [{ type: Schema.Types.ObjectId, ref: 'Classroom' }],
  students: [{ type: Schema.Types.ObjectId, ref: 'Student' }],
  isPublic: {
    type: Boolean,
    default: false
  },
  status: {
    type: String,
    enum: ['draft', 'published', 'archived'],
    default: 'draft'
  },
  isDeleted: {
    type: Boolean,
    default: false
  },
  user: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
    required: true
  }
}, { timestamps: true });
module.exports = model('Test', testSchema);
```
