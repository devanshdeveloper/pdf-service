```javascript
// File: ./features/ticket/ticket.model
const mongoose = require("mongoose");
const ticketSchema = new mongoose.Schema(
  {
    title: {
      type: String,
      required: true,
      minLength: 2,
      name: "Title",
    },
    description: {
      type: String,
      name: "Description",
    },
    priority: {
      type: String,
      enum: ["low", "medium", "high", "critical"],
      default: "medium",
      name: "Priority",
    },
    type: {
      type: String,
      enum: ["bug", "feature", "enhancement", "support"],
      default: "bug",
      name: "Type",
    },
    status: {
      type: String,
      enum: ["open", "in_progress", "testing", "resolved", "closed"],
      default: "open",
      name: "Status",
    }, // The specific component, module, or area of the application where the issue occurs
    affectedArea: {
      type: String,
      name: "Affected Area",
      description:
        "The specific part of the application where the issue is occurring (e.g., login page, dashboard, API endpoint)",
    },
    // Detailed step-by-step instructions to reproduce the issue
    steps: {
      type: String,
      name: "Steps to Reproduce",
      description:
        "Step-by-step instructions that explain how to reproduce the issue",
    },
    // What should happen when following the steps
    expectedBehavior: {
      type: String,
      name: "Expected Behavior",
      description: "What should happen when the feature is working correctly",
    },
    // What actually happens when following the steps
    actualBehavior: {
      type: String,
      name: "Actual Behavior",
      description:
        "What is currently happening instead of the expected behavior",
    },
    // Technical context where the issue occurs
    environment: {
      type: String,
      name: "Environment",
      description:
        "Technical details like browser version, operating system, or environment (dev/staging/prod)",
    },
    attachments: {
      name: "Attachments",
      type: [
        {
          type: String, // URLs or file paths
        },
      ],
    },
    dueDate: {
      name: "Due Date",
      type: Date,
    },
    // assignedTo: {
    //   type: mongoose.Schema.Types.ObjectId,
    //   ref: "User", resolveBy : "username",
    //   name: "Assigned To",
    // },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "User"
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Deleted Status",
    },
  },
  { timestamps: true, strictPopulate: false }
);
const Ticket = mongoose.model("Ticket", ticketSchema);
module.exports = Ticket;

```
