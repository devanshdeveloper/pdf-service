```javascript
// File: ./features/notification/sms/sms-template/sms-template.model
const mongoose = require("mongoose");
const SMSTemplateSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      unique: true, // Ensure template names are unique per user
    },
    content: {
      type: String,
      required: true,
      trim: true,
      maxlength: 160, // Standard SMS length limit
    },
    type: {
      type: String,
      required: true,
      trim: true,
      // Example types: 'otp', 'alert', 'reminder', 'marketing'
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    isDefault: {
      type: Boolean,
      default: false, // Flag for system default templates
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model("SMSTemplate", SMSTemplateSchema);
```
