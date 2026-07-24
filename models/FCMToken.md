```javascript
// File: ./features/firebase/fcm/fcm.model
const mongoose = require("mongoose");
const fcmTokenSchema = new mongoose.Schema(
  {
    token: {
      type: String,
      required: true,
      unique: true,
      name: "FCM Token",
      minLength: 50, // FCM tokens are long strings; adjust if needed
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "User",
    },
  },
  { timestamps: true }
);
const FcmToken = mongoose.model("FcmToken", fcmTokenSchema);
module.exports = FcmToken;

```
