```javascript
// File: ./features/partner/partner.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const { addressSchema } = require("../address/address.model");
const { bankSchema } = require("../party/party.model");
// ---- Sub Schemas --------------------------------------------------
const documentSchema = new Schema(
    {
        label: String,
        value: String,
        identifier: String,
    },
    { _id: false }
);
const identifierSchema = new Schema(
    {
        label: {
            type: String,
            trim: true,
            minlength: 2,
            maxlength: 50,
        },
        value: {
            type: String,
            trim: true,
            maxlength: 100,
        },
    },
    { _id: false }
);
// ---- Main Schema --------------------------------------------------
const PartnerSchema = new Schema(
    {
        // ---- Identity ----
        firstName: { type: String, required: true, trim: true },
        lastName: { type: String, required: true, trim: true },
        email: {
            type: String,
            lowercase: true,
        },
        phone: {
            type: String,
            required: true,
        },
        avatar: {
            type: String,
            name: "Profile Photo",
        },
        identifiers: {
            type: [identifierSchema],
            default: [],
            name: "Identifiers",
        },
        category: {
            type: String,
            name: "Partner Category",
        },
        status: {
            type: String,
            enum: ["active", "inactive", "blocked"],
            default: "active",
        },
        // ---- Ownership / Territory ----
        assignedDistrictOperator: {
            type: Schema.Types.ObjectId,
            ref: "DistrictOperator",
        },
        state: {
            type: String,
            name: "State",
        },
        territory: {
            type: Schema.Types.ObjectId,
            ref: "Territory",
            name: "Territory",
        },
        // ---- Referral ----
        referralCode: {
            type: String,
            unique: true,
        },
        // ---- Commission Rules ----
        commission: {
            type: {
                type: String, // percentage | slab
                enum: ["percentage", "slab"],
                default: "percentage"
            },
            percentage: {
                type: Number,
                default: 0,
                min: 0,
                max: 100
            },
            slabs: [
                {
                    from: {
                        type: Number,
                        required: true,
                        min: 0
                    },
                    to: {
                        type: Number, // null = infinity
                        default: null
                    },
                    percentage: {
                        type: Number,
                        required: true,
                        min: 0,
                        max: 100
                    }
                }
            ],
            calculationBasis: {
                type: String,
                enum: ["monthly", "lifetime"],
                default: "monthly"
            }
        },
        bank: {
            type: bankSchema,
            name: "Bank Details",
        },
        address: {
            type: addressSchema,
            name: "Address",
        },
        documents: {
            type: [documentSchema],
            default: [],
            name: "Documents",
        },
        fields: [
            {
                label: { type: String, required: true },
                value: { type: Schema.Types.Mixed, required: true },
            },
        ],
        // ---- Auth ----
        username: {
            type: String,
            unique: true,
            trim: true,
        },
        password: {
            type: String,
            trim: true,
        },
        userId: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
        // ---- Soft Delete ----
        isDeleted: {
            type: Boolean,
            default: false,
        },
    },
    { timestamps: true }
);
module.exports = mongoose.model("Partner", PartnerSchema);

```
