```javascript
// File: ./features/settings/payment/payment.model
const mongoose = require("mongoose");
const PaymentSettingsSchema = new mongoose.Schema(
  {
    provider: {
      type: String,
      enum: ["razorpay"],
      default: "razorpay",
      required: true,
    },
    liveMode: {
      type: Boolean,
      default: false,
    },
    liveRazorpayKey: {
      type: String,
      required: true,
    },
    liveRazorpaySecret: {
      type: String,
      required: true,
    },
    sandboxRazorpayKey: {
      type: String,
      required: true,
    },
    sandboxRazorpaySecret: {
      type: String,
      required: true,
    },
    razorpayWebhookUrl: {
      type: String,
      required: true,
    },
    razorpaySandboxWebhookUrl: {
      type: String,
      required: true,
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model("PaymentSettings", PaymentSettingsSchema);
```
