```javascript
// File: ./features/bill-of-material/bill-of-material.model
const mongoose = require("mongoose");
const billOfMaterialSchema = new mongoose.Schema(
    {
        number: {
            type: String,
            required: true,
            trim: true,
            name: "Bill of Material Number",
        },
        name: {
            type: String,
            name: "Bill of Material Name",
            required: true,
            minLength: 2,
        },
        description: {
            type: String,
            name: "Bill of Material Description",
            required: true,
            minLength: 2,
        },
        toProduce: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Product",
            resolveBy: "name",
            required: true,
            name: "To Produce Product",
        },
        quantity: {
            type: Number,
            required: true,
            min: 0,
            name: "Quantity",
        },
        unit: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Unit",
            resolveBy: "name",
            required: true,
            name: "Unit",
        },
        components: [
            {
                product: {
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "Product",
                    resolveBy: "name",
                    required: true,
                    name: "Component Product",
                },
                quantity: {
                    type: Number,
                    required: true,
                    min: 0,
                    name: "Quantity",
                },
                wastagePercent: {
                    type: Number,
                    default: 0,
                    min: 0,
                    name: "Wastage Percent",
                },
                unit: {
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "Unit",
                    resolveBy: "name",
                    required: true,
                    name: "Unit",
                },
            }
        ],
        operations: [
            {
                operation: {
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "ManufacturingOperation",
                    resolveBy: "name",
                    required: true,
                    name: "Manufacturing Operation",
                },
                blockedByOperations: [{
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "ManufacturingOperation",
                    resolveBy: "name",
                    name: "Blocked By Operation",
                }],
            }
        ],
        status: {
            type: String,
            enum: ["active", "inactive"],
            default: "active",
            name: "Status",
        },
        isActive: {
            type: Boolean,
            default: true,
            name: "Is Active",
        },
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
    },
    { timestamps: true }
);
const BillOfMaterial = mongoose.model("BillOfMaterial", billOfMaterialSchema);
module.exports = BillOfMaterial;

```
