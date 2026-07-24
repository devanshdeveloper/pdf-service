```javascript
// File: ./features/event/event.model
const mongoose = require("mongoose");
const eventSchema = new mongoose.Schema(
  {
    title: {
      type: String,
      required: true,
      minLength: 2,
    },
    description: {
      type: String,
    },
    start: {
      type: Date,
      required: true,
    },
    end: {
      type: Date,
      required: true,
    },
    allDay: {
      type: Boolean,
      default: false,
    },
    color: {
      type: String,
      enum: ["primary", "secondary", "success", "warning", "danger"],
      default: "primary",
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
    },
    // Common fields for all features
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    isDeleted: {
      type: Boolean,
      default: false,
    },
  },
  { timestamps: true }
);
const Event = mongoose.model("Event", eventSchema);
module.exports = Event;

```
