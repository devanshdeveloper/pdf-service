```javascript
// File: ./features/notification/email/email-template/email-template.model
const mongoose = require("mongoose");
const EmailTemplateSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
    },
    subject: {
      type: String,
      required: true,
      trim: true,
    },
    htmlContent: {
      type: String,
    },
    textContent: {
      type: String, // Optional plain text version
    },
    reactContent: {
      type: String, // Optional React component version
    },
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
module.exports = mongoose.model("EmailTemplate", EmailTemplateSchema);

```
