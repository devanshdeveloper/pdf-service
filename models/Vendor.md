```javascript
// File: ./features/vendor/vendor.model
const mongoose = require("mongoose");
const vendorSchema = new mongoose.Schema(
  {
    // Vendor name
    name: {
      type: String,
      required: true,
      trim: true,
      minLength: 2,
      name: "Vendor Name",
    },
    // Vendor email
    email: {
      type: String,
      trim: true,
      lowercase: true,
      name: "Email",
      validate: {
        validator: function (v) {
          return !v || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v);
        },
        message: "Invalid email format",
      },
    },
    // Vendor phone
    phone: {
      type: String,
      required: true,
      trim: true,
      name: "Phone",
    },
    // GST number
    gst: {
      type: String,
      trim: true,
      uppercase: true,
      name: "GST Number",
      validate: {
        validator: function (v) {
          // Simple GSTIN format validation (15 alphanumeric)
          return !v || /^[0-9A-Z]{15}$/.test(v);
        },
        message: "Invalid GST number format",
      },
    },
    // Common fields
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "User",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
    },
  },
  { timestamps: true }
);
const Vendor = mongoose.model("Vendor", vendorSchema);
module.exports = Vendor;

```
