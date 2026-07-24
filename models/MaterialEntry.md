```javascript
// File: ./features/material-entry/material-entry.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const {
    MaterialEntryTypesArray,
    materialEntryTypes,
    MaterialEntryStatusTypesArray,
} = require("./material-entry");

const MaterialEntrySchema = new Schema(
    {
        number: {
            type: String,
            required: true,
            trim: true,
            name: "Material Entry Number",
        },
        date: {
            type: Date,
            required: true,
            name: "Date",
        },
        referenceNumber: {
            type: String,
            trim: true,
            name: "Reference Number",
        },
        location: {
            type: Schema.Types.ObjectId,
            ref: "InventoryLocation",
            required: true,
            name: "Location",
        },
        products: [
            {
                product: {
                    type: Schema.Types.ObjectId,
                    ref: "Product",
                    required: true,
                    name: "Product",
                },
                name: {
                    type: String,
                    trim: true,
                    name: "Product Name",
                },
                quantity: {
                    type: Number,
                    required: true,
                    min: 0,
                    name: "Quantity",
                },
                unit: {
                    type: Schema.Types.ObjectId,
                    ref: "Unit",
                    name: "Unit",
                },
            },
        ],
        notes: {
            type: String,
            trim: true,
            name: "Notes",
        },
        terms: {
            type: String,
            trim: true,
            name: "Terms",
        },
        signature: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Signature",
            trim: true,
            name: "Signature",
        },
        status: {
            type: String,
            enum: MaterialEntryStatusTypesArray,
            default: "Pending",
            name: "Status",
        },
        type: {
            type: String,
            enum: MaterialEntryTypesArray,
            default: materialEntryTypes.MaterialEntry,
            name: "Type",
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
            name: "User",
        },
        nextVoucher: {
            type: Schema.Types.ObjectId,
            ref: "Voucher",
            name: "Next Voucher",
        },
        previousVoucher: {
            type: Schema.Types.ObjectId,
            ref: "Voucher",
            name: "Previous Voucher",
        },
        nextMaterialEntry: {
            type: Schema.Types.ObjectId,
            ref: "MaterialEntry",
            name: "Next Material Entry",
        },
        previousMaterialEntry: {
            type: Schema.Types.ObjectId,
            ref: "MaterialEntry",
            name: "Previous Material Entry",
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted",
        },
    },
    {
        timestamps: true,
    }
);
const MaterialEntry = mongoose.model("MaterialEntry", MaterialEntrySchema);
module.exports = MaterialEntry;

```
