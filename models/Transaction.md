```javascript
// File: ./features/transaction/transaction.model
const mongoose = require("mongoose");
const PaymentMethods = require("../../data/PaymentMethods");
const { Schema } = mongoose;
// ========== TransactionType Enum ==========
const TransactionType = Object.freeze({
  INCOME: "income",
  EXPENSE: "expense",
});
// ========== Transaction Schema ==========
const transactionSchema = new Schema(
  {
    type: {
      type: String,
      required: true,
      enum: Object.values(TransactionType),
    },
    name: {
      type: String,
      trim: true,
    },
    ledgerHead: {
      type: Schema.Types.ObjectId,
      ref: "LedgerHead",
      required: true,
    },
    date: {
      type: Date,
      required: true,
    },
    amount: {
      type: Number,
      required: true,
      min: [0, "Amount must be non-negative"],
    },
    mode: {
      type: String,
      required: true,
      enum: Object.values(PaymentMethods),
    },
    party: {
      type: Schema.Types.ObjectId,
      ref: "Party",
      required: true,
    },
    bank: {
      type: Schema.Types.ObjectId,
      ref: "Bank",
    },
    remarks: {
      type: String,
      trim: true,
    },
    attachments: [
      {
        type: String,
        trim: true,
      },
    ],
    approvedBy: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
    },
    rejectedBy: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
    },
    comments: [{
      message: {
        type: String,
        trim: true,
      },
      author: {
        type: Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
      },
      createdAt: {
        type: Date,
        default: Date.now,
      },
    }],
    status: {
      type: String,
      default: "draft",
      enum: ["draft", "pending", "approved", "rejected"],
    },
    createdBy: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    isDeleted: {
      type: Boolean,
      default: false,
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  {
    timestamps: true,
  }
);
// attach enums to statics
module.exports = mongoose.model("Transaction", transactionSchema);

```
