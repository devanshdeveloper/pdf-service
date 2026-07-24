```javascript
// File: ./features/bank/bank.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const bankSchema = new Schema(
  {
    name: {
      type: Schema.Types.String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 100,
      name: "Account Holder Name"
    },
    bankName: {
      type: Schema.Types.String,
      trim: true,
      maxlength: 100,
      name: "Bank Name"
    },
    branch: {
      type: Schema.Types.String,
      required: true,
      trim: true,
      maxlength: 100,
      name: "Branch Name"
    },
    branchAddress: {
      type: Schema.Types.String,
      trim: true,
      maxlength: 200,
      name: "Branch Address"
    },
    accountType: {
      type: Schema.Types.String,
      trim: true,
      enum: ["Savings", "Current", "Salary"],
      default: "Savings",
      name: "Account Type"
    },
    accountNumber: {
      type: Schema.Types.String,
      required: true,
      trim: true,
      validate: {
        validator: function (v) {
          return /^\d{9,18}$/.test(v);
        },
        message: "Account number must be between 9 and 18 digits"
      },
      name: "Account Number"
    },
    ifsc: {
      type: Schema.Types.String,
      required: true,
      trim: true,
      validate: {
        validator: function (v) {
          return /^[A-Z]{4}0[A-Z0-9]{6}$/.test(v);
        },
        message: "Invalid IFSC code format"
      },
      name: "IFSC Code"
    },
    paymentPrefrence: {
      type: Schema.Types.String,
      enum: ["NEFT", "RTGS", "IMPS"],
      default: "NEFT",
      name: "Payment Preference"
    },
    balance: {
      type: Number,
      default: 0,
      min: 0,
      name: "Balance"
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted"
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  { timestamps: true }
);
const Bank = mongoose.model("Bank", bankSchema);
module.exports = Bank;
```
