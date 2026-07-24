```javascript
// File: ./features/settings/voucher/voucher-settings.model
const mongoose = require("mongoose");
const { VoucherTypesArray } = require("../../voucher/voucher");
const VoucherSettingsSchema = new mongoose.Schema(
    {
        // Display name for this invoice settings profile
        name: {
            type: String,
            required: true,
            trim: true,
            minLength: 2,
            name: "Voucher Settings Name",
        },
        // Company or brand logo (URL or file reference)
        logo: {
            type: String,
            name: "Logo",
        },

        defaults: {
            terms: {
                type: String,
                name: "Terms",
            },
            notes: {
                type: String,
                name: "Notes",
            },
            bank: {
                type: mongoose.Schema.Types.ObjectId,
                ref: "Bank",
                name: "Bank"
            },
            signature: {
                type: mongoose.Schema.Types.ObjectId,
                ref: "Signature",
                name: "Signature"
            }
        },
        template: {
            type: String,
            trim: true,
            maxlength: 100,
            required: true,
            name: "Template",
        },
        prefix: {
            type: String,
            name: "Prefix"
        },
        counter: {
            type: Number,
            name: "Counter"
        },
        type: {
            type: String,
            required: true,
            enum: VoucherTypesArray,
            name: "Type",
        },
        status: {
            type: String,
            enum: ["active", "inactive"],
            default: "active",
            name: "Status"
        },
        // Common fields
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
            name: "User",
        },
        business: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Business",
            name: "Business",
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted",
        },
        defaultCurrency: {
            type: String,
            name: "Default Currency",
        },
    },
    { timestamps: true }
);
const VoucherSettings = mongoose.model(
    "VoucherSettings",
    VoucherSettingsSchema
);
module.exports = VoucherSettings;

```
