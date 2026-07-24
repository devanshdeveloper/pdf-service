```javascript
// File: ./features/settings/ledger/ledger-settings.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const ledgerSettingSchema = new Schema(
  {
    heads: {
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
const LedgerSetting = mongoose.model("LedgerSetting", ledgerSettingSchema);
module.exports = LedgerSetting;

```
