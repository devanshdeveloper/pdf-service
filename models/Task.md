```javascript
// File: ./features/task/task.model
const mongoose = require("mongoose");
const taskSchema = new mongoose.Schema(
  {
    title: {
      type: String,
      required: true,
      trim: true,
      minLength: 2,
      name: "Task Title",
    },
    description: {
      type: String,
      trim: true,
      name: "Description",
    },
    status: {
      type: String,
      enum: ["todo", "in_progress", "review", "done", "blocked"],
      default: "todo",
      name: "Status",
    },
    priority: {
      type: String,
      enum: ["low", "medium", "high", "urgent"],
      default: "medium",
      name: "Priority",
    },
    dueDate: {
      type: Date,
      name: "Due Date",
    },
    // Relations
    project: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Project",
      required: true,
      name: "Project",
    },
    assignedTo: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      name: "Assigned To",
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "Created By",
    },
    // Tracking
    comments: [
      {
        user: { type: mongoose.Schema.Types.ObjectId, ref: "User" },
        text: { type: String, trim: true },
        createdAt: { type: Date, default: Date.now },
      },
    ],
    attachments: [
      {
        url: String,
        filename: String,
        uploadedAt: { type: Date, default: Date.now },
      },
    ],
    // Common
    isDeleted: { type: Boolean, default: false },
  },
  { timestamps: true }
);
const Task = mongoose.model("Task", taskSchema);
module.exports = Task;

```
