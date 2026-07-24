```javascript
// File: ./features/party/party.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");
const { Schema } = mongoose;
const bankSchema = new Schema(
    {
        name: {
            type: Schema.Types.String,
            trim: true,
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
            trim: true,
            validate: {
                validator: function (v) {
                    if (!v) return true;
                    return /^\d{9,18}$/.test(v);
                },
                message: "Account number must be between 9 and 18 digits"
            },
            name: "Account Number"
        },
        ifsc: {
            type: Schema.Types.String,
            trim: true,
            validate: {
                validator: function (v) {
                    if (!v) return true;
                    return /^[A-Z]{4}0[A-Z0-9]{6}$/.test(v);
                },
                message: "Invalid IFSC code format"
            },
            name: "IFSC Code"
        },
    },
    { _id: false }
);
const documentSchema = new Schema(
    {
        label: {
            type: String,
            name: "Document Label",
        },
        value: {
            type: String,
            name: "Document URL",
        },
        identifier: {
            type: String,
            name: "Document Identifier"
        }
    },
    { _id: false }
);
const PartySchema = new Schema(
    {
        // Basic Details
        businessType: {
            type: String,
            enum: ["Sole Proprietorship", "Partnership", "Company", "Individual"],
            default: "individual",
            name: "Business Type",
        },
        avatar: {
            type: String,
            name: "Avatar"
        },
        firstName: {
            type: String,
            required: true,
            trim: true,
            name: "Party First Name",
        },
        lastName: {
            type: String,
            required: true,
            trim: true,
            name: "Party Last Name",
        },
        phone: { type: String, trim: true, name: "Phone Number" },
        email: {
            type: String,
            trim: true,
            lowercase: true,
            match: [/^\S+@\S+\.\S+$/, "Invalid email address"],
            name: "Email",
        },
        businessName: { type: String, trim: true, name: "Business Name" },
        head: { type: String, trim: true, name: "Head" },
        dateOfBirth: {
            type: Date,
            name: "Date of Birth",
        },
        gender: {
            type: String,
            enum: ["Male", "Female", "Other"],
            name: "Gender",
        },
        // Addresses
        billingAddress: { type: addressSchema, name: "Billing Address" },
        shippingAddress: { type: addressSchema, name: "Shipping Address" },
        preferredPaymentTerms: {
            type: String,
            enum: ["Cash", "NEFT", "UPI", "RTGS", "IMPS"],
            name: "Preferred Payment Terms",
        },
        // Bank Details
        bank: { type: bankSchema, name: "Bank Details" },
        // Optional misc fields seen on many forms
        notes: { type: String, trim: true, name: "Notes" },
        gst: { type: String, trim: true, name: "GST Number" },
        fields: [
            {
                label: {
                    type: String,
                    required: true,
                },
                value: {
                    type: Schema.Types.Mixed,
                    required: true,
                },
            },
        ],
        documents: {
            type: [documentSchema],
            default: [],
            name: "Documents",
        },
        username: {
            type: String,
            trim: true,
            unique: true,
            sparse: true,
            name: "Username"
        },
        password: {
            type: String,
            name: "Password"
        },
        userId: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            name: "User ID"
        },
        // Admin / relations
        user: { type: Schema.Types.ObjectId, ref: "User", resolveBy: "username", required: true, name: "User" },
        status: {
            type: String,
            enum: ["active", "inactive", "pending", "draft"],
            default: "active",
            name: "Status",
        },
        isDeleted: { type: Boolean, default: false, name: "Is Deleted" },
    },
    { timestamps: true }
);
const Party = mongoose.model("Party", PartySchema);
module.exports = Party;
module.exports.bankSchema = bankSchema;
```
