```javascript
// File: ./features/work-order/work-order.model
const mongoose = require("mongoose");
const workOrderSchema = new mongoose.Schema(
    {
        number: {
            type: String,
            required: true,
            trim: true,
            name: "Work Order Number",
        },
        name: {
            type: String,
            name: "Work Order Name",
            required: true,
            minLength: 2,
        },
        business: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Business",
            resolveBy: "name",
            required: true,
            name: "Business",
        },
        billOfMaterial: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "BillOfMaterial",
            resolveBy: "name",
            required: true,
            name: "Bill of Material",
        },
        description: {
            type: String,
            name: "Work Order Description",
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
                unit: {
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "Unit",
                    resolveBy: "name",
                    required: true,
                    name: "Unit",
                },
            }
        ],
        frozenCostBreakdown: [
            {
                componentProduct: {
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "Product",
                    resolveBy: "name",
                },
                quantityUsed: { type: Number },
                unitCost: { type: Number },
                totalCost: { type: Number },
                unit: {
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "Unit",
                    resolveBy: "name",
                }
            }
        ],
        frozenTotalCost: {
            type: Number,
            name: "Frozen Total Cost",
        },
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
            enum: ["draft", "planned", "in-progress", "completed", "cancelled"],
            default: "draft",
            name: "Status",
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
const WorkOrder = mongoose.model("WorkOrder", workOrderSchema);
module.exports = WorkOrder;

```
