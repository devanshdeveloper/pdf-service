```javascript
// File: ./features/notification/mobile-notification-config/mobile-notification-config.model
const mongoose = require("mongoose");
const MobileNotificationSettingsSchema = new mongoose.Schema(
  {
    provider: {
      type: String,
      enum: ["fcm", "apns"], // Firebase Cloud Messaging, Apple Push Notification Service
      required: true,
    },
    apiKey: { // Could be Server Key for FCM, or Auth Key for APNS
      type: String,
      required: true,
    },
    // Add other provider-specific fields as needed
    // For FCM:
    // projectId: { type: String },
    // For APNS:
    // keyId: { type: String },
    // teamId: { type: String },
    // bundleId: { type: String },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      unique: true, // Typically one mobile config per user/provider combo
    },
    isEnabled: {
      type: Boolean,
      default: true,
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model(
  "MobileNotificationSettings",
  MobileNotificationSettingsSchema
);
```
