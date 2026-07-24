```javascript
// File: ./features/inventory-transaction/inventory-transaction.model
const mongoose = require("mongoose");
const inventoryTransactionSchema = new mongoose.Schema(
    {
        product: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Product",
            required: true,
            name: "Product",
        },
        quantity: {
            type: Number,
            required: true,
            min: [0, "Quantity must be at least 1"],
            name: "Quantity",
        },
        change: {
            type: Number,
            default: 0,
            name: "Change",
        },
        transactionType: {
            type: String,
            enum: ["initial", "purchase", "sale", "return", "adjustment"],
            required: true,
            name: "Transaction Type",
        },
        notes: {
            type: String,
            maxlength: 500,
            name: "Notes",
        },
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
const InventoryTransaction = mongoose.model(
    "InventoryTransaction",
    inventoryTransactionSchema
);
module.exports = InventoryTransaction;

```
