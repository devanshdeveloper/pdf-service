```javascript
// File: ./features/inventory-location/inventory-location.model
const mongoose = require("mongoose");
const inventoryLocationSchema = new mongoose.Schema(
    {
        name: {
            type: String,
            required: true,
            name: "Name",
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
const InventoryLocation = mongoose.model(
    "InventoryLocation",
    inventoryLocationSchema
);
module.exports = InventoryLocation;

```
