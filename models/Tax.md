```javascript
// File: ./features/tax/tax.model
const mongoose = require("mongoose");
const taxSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      name: "Tax Name",
      required: true,
      minLength: 2,
    },
    value: {
      type: Number,
      name: "Tax Value",
      required: true,
      min: 0,
      max: 100, // assuming percentage tax
    },
    level: {
      type: String,
      enum: ["Product", "Voucher"],
      required: true,
      name: "Level"
    },
    type: {
      type: String,
      enum: ["GST", "IGST", "CGST", "SGST", "UTGST"],
      required: true,
      name: "Type"
    },
    appliesTo: {
      type: String,
      enum: ["all", "intra-state", "inter-state", "union-territory"],
      name: "AppliesTo"
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
const Tax = mongoose.model("Tax", taxSchema);
module.exports = Tax;

```
