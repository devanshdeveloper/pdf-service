```javascript
// File: ./features/district-operator/district-operator.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const { addressSchema } = require("../address/address.model");
const { bankSchema } = require("../party/party.model");
const PaymentMethods = require("../../data/PaymentMethods");
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
const DistrictOperatorSchema = new Schema(
    {
        // ---- Identity ----
        firstName: { type: String, trim: true },
        lastName: { type: String, trim: true },
        email: {
            type: String,
            lowercase: true,
            unique: true,
        },
        phone: {
            type: String,
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
        // ---- Territories ----
        territories: [
            {
                state: { type: String, },
                district: { type: String },
                exclusive: { type: Boolean, default: true },
            },
        ],
        // ---- Commission ----
        commission: {
            type: {
                type: String,
                enum: ["percentage", "slab"],
                default: "percentage",
            },
            value: {
                type: Number,
            },
            slabs: [
                {
                    from: { type: Number, },
                    to: { type: Number }, // null = infinity
                    percentage: { type: Number, },
                },
            ],
        },
        preferredPaymentMethod: {
            type: String,
            enum: Object.values(PaymentMethods),
            default: PaymentMethods.Cash,
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
                label: { type: String, },
                value: { type: Schema.Types.Mixed, },
            },
        ],
        // ---- Auth ----
        username: { type: String, unique: true },
        password: { type: String, },
        userId: { type: mongoose.Schema.Types.ObjectId, ref: "User", unique: true },
        status: {
            type: String,
            enum: ["active", "inactive", "suspended"],
            default: "active",
        },
        // ---- Soft Delete ----
        isDeleted: { type: Boolean, default: false },
    },
    { timestamps: true }
);
module.exports = mongoose.model("DistrictOperator", DistrictOperatorSchema);

```
