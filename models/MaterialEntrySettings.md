```javascript
// File: ./features/settings/material-entry/material-entry-setting.model
const mongoose = require("mongoose");
const { MaterialEntryTypesArray } = require("../../material-entry/material-entry");

const MaterialEntrySettingSchema = new mongoose.Schema(
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
        template: {
            type: String,
            trim: true,
            maxlength: 100,
            required: true,
            name: "Template",
        },
                pdf_template: {
            type: String,
            trim: true,
            maxlength: 100,
            required: true,
            name: "PDF Template",
            default: "Standard"
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
        type: {
            type: String,
            required: true,
            enum: MaterialEntryTypesArray,
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
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted",
        },
    },
    { timestamps: true }
);
const MaterialEntrySettings = mongoose.model(
    "MaterialEntrySettings",
    MaterialEntrySettingSchema
);
module.exports = MaterialEntrySettings;

```
