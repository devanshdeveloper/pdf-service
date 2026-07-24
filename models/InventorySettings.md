```javascript
// File: ./features/settings/inventory/inventory-settings.model
const mongoose = require("mongoose");
const inventorySettingsSchema = new mongoose.Schema(
  {
    // Add stock when a purchase is created
    addStockOnPurchase: {
      type: Boolean,
      default: true,
      name: "Add Stock on Purchase",
    },
    // Remove stock when an invoice is created
    removeStockOnInvoice: {
      type: Boolean,
      default: true,
      name: "Remove Stock on Invoice",
    },
    // Add stock on sales return
    addStockOnSalesReturn: {
      type: Boolean,
      default: true,
      name: "Add Stock on Sales Return",
    },
    // Remove stock on purchase return
    removeStockOnPurchaseReturn: {
      type: Boolean,
      default: true,
      name: "Remove Stock on Purchase Return",
    },
    // Remove stock on delivery challan
    removeStockOnDeliveryChallan: {
      type: Boolean,
      default: false,
      name: "Remove Stock on Delivery Challan",
    },
    // Common fields
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
const InventorySettings = mongoose.model(
  "InventorySettings",
  inventorySettingsSchema
);
module.exports = InventorySettings;

```
