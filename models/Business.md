```javascript
// File: ./features/business/business.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");
const { Schema } = mongoose;
// Complex validator: check if website URL is valid
function validateWebsite(url) {
  const regex =
    /^(https?:\/\/)?([a-zA-Z0-9.-]+)\.[a-zA-Z]{2,}(\/[^\s]*)?$/;
  return regex.test(url);
}
const businessSchema = new Schema(
  {
    name: {
      type: String,
      name: "Business Name",
      minLength: 2,
      maxLength: 100,
    },
    description: {
      type: String,
      name: "Description",
      maxLength: 500,
    },
    email: {
      type: String,
      name: "Business Email",
      match: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, // inline simple validator
    },
    phone: {
      type: String,
      name: "Phone Number",
      match: /^[0-9+\-()\s]{7,20}$/, // inline simple validator
    },
    website: {
      type: String,
      name: "Website",
      validate: [validateWebsite, "Invalid website URL"], // extracted validator
    },
    address: addressSchema,
    logo: {
      type: String,
      name: "Logo URL",
    },
    gst: {
      type: String,
      name: "GST Number",
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
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
    baseCurrency: {
      type: String,
      required: true,
      default: "INR",
      name: "Base Currency"
    },
  },
  { timestamps: true }
);
const Business = mongoose.model("Business", businessSchema);
module.exports = Business;

```
