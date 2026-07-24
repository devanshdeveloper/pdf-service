```javascript
// File: ./features/discount/discount.model
const mongoose = require("mongoose");
const discountSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      name: "Discount Name",
      required: true,
      minLength: 2,
    },
    type: {
      type: String,
      name: "Discount Type",
      enum: ["percentage", "flat"],
      required: true,
    },
    value: {
      type: Number,
      name: "Discount Value",
      required: true,
      min: 0,
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
const Discount = mongoose.model("Discount", discountSchema);
module.exports = Discount;

```
