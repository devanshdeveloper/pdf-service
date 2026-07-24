```javascript
// File: ./features/Institute/Institute.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");
const { Schema } = mongoose;
const instituteSchema = new Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 100,
      name: "Institute Name"
    },
    logo: {
      type: String,
      trim: true,
      name: "Logo"
    },
    email: {
      type: String,
      required: true,
      trim: true,
      lowercase: true,
      name: "Email Address"
    },
    phone: {
      type: String,
      required: true,
      trim: true,
      name: "Phone Number"
    },
    registrationNumber: {
      type: String,
      required: true,
      unique: true,
      trim: true,
      name: "Registration Number"
    },
    website: {
      type: String,
      trim: true,
      name: "Website"
    },
    foundedYear: {
      type: Number,
      min: 1800,
      max: new Date().getFullYear(),
      name: "Founded Year"
    },
    address: addressSchema,
    type: {
      type: String,
      enum: ["School", "College", "University", "Other"],
      default: "School",
      name: "Institute Type"
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Deleted Status"
    },
    user: {
      type: Schema.Types.ObjectId,
      show: false,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  {
    timestamps: true,
  }
);
const Institute = mongoose.model("Institute", instituteSchema);
module.exports = Institute;

```
