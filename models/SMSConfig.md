```javascript
// File: ./features/notification/sms/sms-config/sms-config.model
const mongoose = require("mongoose");
const SMSSettingsSchema = new mongoose.Schema(
  {
    provider: {
      type: String,
      enum: ["twilio", "messagebird", "vonage"],
      required: true,
    },
    senderNumber: {
      type: String,
      required: true,
    },
    apiKey: {
      type: String,
      required: true,
    },
    expiryTime: {
      type: Number,
      required: true,
      min: 1,
      max: 60,
      default: 5,
    },
    otpLength: {
      type: Number,
      required: true,
      min: 4,
      max: 8,
      default: 6,
    },
    retryLimit: {
      type: Number,
      required: true,
      min: 1,
      max: 10,
      default: 3,
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model("SMSSettings", SMSSettingsSchema);
```
