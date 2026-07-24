```javascript
// File: ./features/subject/subject.model
const { Schema, model } = require("mongoose");
const Statuses = require("../../constants/Statuses");
const subjectSchema = new Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      unique: false,
      name: "Subject Name",
    },
    code: {
      type: String,
      required: true,
      trim: true,
      name: "Subject Code",
    },
    stream: {
      type: Schema.Types.ObjectId,
      ref: "Stream", resolveBy: "name",
      name: "Stream",
    },
    description: {
      type: String,
      trim: true,
      name: "Description",
    },
    syllabus: {
      type: String,
      trim: true,
      name: "Syllabus",
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
      name: "Status",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      name: "User",
      show: false,
      required: true,
    }
  },
  {
    timestamps: true,
  }
);
const Subject = model("Subject", subjectSchema);
module.exports = Subject;

```
