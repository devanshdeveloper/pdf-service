```javascript
// File: ./features/notification/notification.model
const mongoose = require("mongoose");
const notificationSchema = new mongoose.Schema(
  {
    users: [
      {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true,
      },
    ],
    readBy: [
      {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
      },
    ],
    title: {
      type: String,
      required: true,
      trim: true,
    },
    description: {
      type: String,
      required: true,
      trim: true,
    },
    redirect: {
      type: String,
      trim: true,
    },
    icon: {
      type: String,
    },
    color: {
      type: String,
      enum: ["primary", "danger", "success", "warning", "default"],
      default: "primary",
    },
    avatar: {
      type: String,
      trim: true,
    },
  },
  { timestamps: true }
);
const Notification = mongoose.model("Notification", notificationSchema);
module.exports = Notification;

```
