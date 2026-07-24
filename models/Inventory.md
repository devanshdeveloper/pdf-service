```javascript
// File: ./features/inventory/inventory.model
const mongoose = require("mongoose");
const inventorySchema = new mongoose.Schema(
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
        location: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "InventoryLocation",
            required: true,
            name: "Location",
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
const Inventory = mongoose.model(
    "Inventory",
    inventorySchema
);
module.exports = Inventory;

```
