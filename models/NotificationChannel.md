```javascript
// File: ./features/notification/notification-channel/notification-channel.model
const mongoose = require("mongoose");
const NotificationChannelSchema = new mongoose.Schema(
  {
    event: {
      type: String,
      required: true,
    },
    title: {
      type: String,
      required: true,
    },
    description: {
      type: String,
      required: true,
    },
    icon: {
      type: String,
      required: true,
    },
    color: {
      type: String,
      required: true,
    },
    channels: {
      type: [
        {
          type: String,
          required: true,
          enum: ["email", "sms", "push", "in-app", "whatsapp"],
        },
      ],
      default: ["in-app"]
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model(
  "NotificationChannel",
  NotificationChannelSchema
);

```
