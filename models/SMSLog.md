```javascript
// File: ./features/notification/sms/sms-log/sms-log.model
const mongoose = require("mongoose");
const SMSLogSchema = new mongoose.Schema(
  {
    recipient: {
      type: String,
      required: true,
      trim: true,
    },
    content: {
      type: String,
      required: true,
      trim: true,
    },
    status: {
      type: String,
      enum: ["pending", "sent", "delivered", "failed", "undelivered"],
      default: "pending",
      required: true,
    },
    provider: {
      type: String, // e.g., 'twilio', 'messagebird'
      required: true,
    },
    providerMessageId: {
      type: String, // ID returned by the SMS provider
    },
    failureReason: {
      type: String, // Store error message if status is 'failed'
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    template: { // Optional: Link to the template used
      type: mongoose.Schema.Types.ObjectId,
      ref: "SMSTemplate",
    },
    sentAt: {
      type: Date,
    },
    deliveredAt: {
      type: Date,
    },
  },
  { timestamps: true } // Adds createdAt and updatedAt
);
module.exports = mongoose.model("SMSLog", SMSLogSchema);
```
