```javascript
// File: ./features/material/material.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const materialSchema = new mongoose.Schema(
  {
    title: {
      type: String,
      required: true,
      trim: true,
    },
    description: {
      type: String,
      trim: true,
    },
    subject: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Subject", resolveBy: "name",
      required: true,
    },
    files: [
      {
        type: mongoose.Schema.Types.String,
        required: true,
      }
    ],
    datePublished: { type: Date },
    publishLevel: {
      type: String,
      enum: ['course', 'department', 'classroom', 'specific'],
      required: true
    },
    courses: [{ type: Schema.Types.ObjectId, ref: 'Course' }],
    departments: [{ type: Schema.Types.ObjectId, ref: 'Department' }],
    classrooms: [{ type: Schema.Types.ObjectId, ref: 'Classroom' }],
    students: [{ type: Schema.Types.ObjectId, ref: 'Student' }],
    isDeleted: {
      type: Boolean,
      default: false,
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true
    }
  },
  {
    timestamps: true,
  }
);
module.exports = mongoose.model("Material", materialSchema);

```
