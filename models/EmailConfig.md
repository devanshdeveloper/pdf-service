```javascript
// File: ./features/notification/email/email-config/email-config.model
const mongoose = require("mongoose");
const EmailConfigSchema = new mongoose.Schema(
  {
    smtpHost: {
      type: String,
      required: true,
    },
    smtpPort: {
      type: Number,
      required: true,
    },
    smtpUsername: {
      type: String,
      required: true,
    },
    smtpPassword: {
      type: String,
      required: true,
    },
    fromEmail: {
      type: String,
      required: true,
    },
    fromName: {
      type: String,
      required: true,
    },
    encryption: {
      type: String,
      enum: ["none", "ssl", "tls"],
      default: "tls",
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model("EmailConfig", EmailConfigSchema);

```
