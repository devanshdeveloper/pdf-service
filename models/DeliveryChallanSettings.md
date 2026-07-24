```javascript
// File: ./features/settings/delivery-challan/delivery-challan-settings.model
const mongoose = require("mongoose");
const DeliveryChallanSettingsSchema = new mongoose.Schema(
    {
        // Display name for this invoice settings profile
        name: {
            type: String,
            required: true,
            trim: true,
            minLength: 2,
            name: "Invoice Settings Name",
        },
        // Company or brand logo (URL or file reference)
        logo: {
            type: String,
            name: "Logo",
        },
        rightContent: {
            type: String,
            name: "Right Content",
        },
        template: {
            type: String,
            required: true,
            enum: ["classic", "modern", "minimal", "custom"], // expand as needed
            default: "classic",
            name: "Template",
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
        prefix: {
            type: String,
            name: "Prefix"
        },
        counter: {
            type: Number,
            name: "Counter"
        },
        // Common fields
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
            name: "User",
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted",
        },
    },
    { timestamps: true }
);
const DeliveryChallanSettings = mongoose.model(
    "DeliveryChallanSettings",
    DeliveryChallanSettingsSchema
);
module.exports = DeliveryChallanSettings;

```
