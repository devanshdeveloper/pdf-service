```javascript
// File: ./features/settings/transaction/transaction-settings.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const transactionSettingSchema = new Schema(
  {
    incomeCategories: {
      type: [
        {
          name: {
            type: String,
            required: true,
            trim: true,
          },
          icon: {
            type: String,
            trim: true,
          },
          color: {
            type: String,
            required: true,
            trim: true,
          },
        },
      ],
      required: true,
    },
    expenseCategories: {
      type: [
        {
          name: {
            type: String,
            required: true,
            trim: true,
          },
          icon: {
            type: String,
            trim: true,
          },
          color: {
            type: String,
            required: true,
            trim: true,
          },
        },
      ],
      required: true,
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
const TransactionSetting = mongoose.model(
  "TransactionSetting",
  transactionSettingSchema
);
module.exports = TransactionSetting;

```
