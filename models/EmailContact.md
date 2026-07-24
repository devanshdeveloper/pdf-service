```javascript
// File: ./features/notification/email/email-contact/EmailContact.model
const mongoose = require("mongoose");
const EmailContactSchema = new mongoose.Schema(
  {
    email: {
      type: String,
      required: true,
      trim: true,
      lowercase: true,
      match: [/^\S+@\S+\.\S+$/, "Please enter a valid email address"],
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  {
    timestamps: true,
  }
);
const EmailContact = mongoose.model("EmailContact", EmailContactSchema);
module.exports = EmailContact;

```
