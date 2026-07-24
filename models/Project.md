```javascript
// File: ./features/project/project.model
const mongoose = require("mongoose");
const projectSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      minLength: 3,
      name: "Project Name",
    },
    description: {
      type: String,
      trim: true,
      name: "Description",
    },
    status: {
      type: String,
      enum: ["planned", "in_progress", "on_hold", "completed", "cancelled"],
      default: "planned",
      name: "Status",
    },
    startDate: {
      type: Date,
      name: "Start Date",
    },
    endDate: {
      type: Date,
      name: "End Date",
    },
    priority: {
      type: String,
      enum: ["low", "medium", "high", "critical"],
      default: "medium",
      name: "Priority",
    },
    // Relations
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "Owner",
    },
    teamMembers: [
      {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        name: "Team Members",
      },
    ],
    // Common
    isDeleted: { type: Boolean, default: false },
  },
  { timestamps: true }
);
const Project = mongoose.model("Project", projectSchema);
module.exports = Project;

```
