```javascript
// File: ./features/stock-transfer-out/stock-transfer-out.model
const mongoose = require("mongoose");
const stockTransferOutSchema = new mongoose.Schema(
    {
        name: {
            type: String,
            name: "Tax Name",
            required: true,
            minLength: 2,
        },
        description: {
            type: String,
            trim: true,
        },
        fromLocation: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "InventoryLocation", resolveBy: "name",
            required: true,
            name: "From Location",
        },
        toLocation: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "InventoryLocation", resolveBy: "name",
            required: true,
            name: "To Location",
        },
        items: [
            {
                type: mongoose.Schema.Types.ObjectId,
                ref: "Product", resolveBy: "name",
                required: true,
                name: "Product Item",
            },
            {
                type: Number,
                name: "Quantity",
                required: true,
                min: 1,
            }
        ],
        attachments: [{
            type: String,
            trim: true
        }],
        requestedBy: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
        approvedBy: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
        },
        rejectedBy: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
        },
        comments: [{
            message: {
                type: String,
                trim: true,
            },
            author: {
                type: mongoose.Schema.Types.ObjectId,
                ref: "User", resolveBy: "username",
            },
            createdAt: {
                type: Date,
                default: Date.now,
            },
        }],
        status: {
            type: String,
            enum: ["draft", "pending", "approved", "rejected"],
            default: "draft",
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
const StockTransferOut = mongoose.model("StockTransferOut", stockTransferOutSchema);
module.exports = StockTransferOut;

```
