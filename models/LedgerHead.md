```javascript
// File: ./features/ledger-head/ledger-head.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const ledgerHeadSchema = new Schema(
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
        referenceType: {
            type: String,
            enum: ["Party", "Bank"],
        },
        party: {
            type: Schema.Types.ObjectId,
            ref: "Party",
        },
        bank: {
            type: Schema.Types.ObjectId,
            ref: "Bank",
        },
        status: {
            type: String,
            enum: ["active", "inactive"],
            default: "active",
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
const LedgerHead = mongoose.model("LedgerHead", ledgerHeadSchema);
module.exports = LedgerHead;

```
