```javascript
// File: ./features/notification/email/email-log/email-log.model
const mongoose = require("mongoose");
const EmailLogSchema = new mongoose.Schema(
  {
    recipients: [
      {
        type: String,
        trim: true,
        lowercase: true,
      },
    ],
    cc: [
      {
        type: String,
        trim: true,
        lowercase: true,
      },
    ],
    bcc: [
      {
        type: String,
        trim: true,
        lowercase: true,
      },
    ],
    subject: {
      type: String,
      required: true,
      trim: true,
    },
    content: {
      type: String,
      required: true,
      trim: true,
    },
    contentType: {
      type: String,
      enum: ["html", "text", "react"],
      default: "html",
      required: true,
    },
    attachments: [
      {
        filename: {
          type: String,
          required: true,
          trim: true,
        },
        contentUrl: {
          type: String, // Store binary data of the attachment
          required: true,
        },
        contentType: {
          type: String,
          required: true,
          trim: true,
        },
      },
    ],
    status: {
      type: String,
      enum: ["pending", "sent", "delivered", "failed", "bounced"],
      default: "pending",
      required: true,
    },
    providerResponse: {
      // Store response or message ID from the email provider
      type: mongoose.Schema.Types.Mixed,
    },
    failureReason: {
      type: String, // Store error message if status is 'failed' or 'bounced'
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    template: {
      // Optional: Link to the email template used
      type: mongoose.Schema.Types.ObjectId,
      ref: "EmailTemplate",
    },
    // Add fields for tracking opens, clicks if needed later
    // openedAt: { type: Date },
    // clickedAt: { type: Date },
  },
  { timestamps: true } // Adds createdAt and updatedAt
);
module.exports = mongoose.model("EmailLog", EmailLogSchema);

```
