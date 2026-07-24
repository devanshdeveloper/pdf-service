```javascript
// File: ./features/cost-template/cost-template.model
const mongoose = require("mongoose");
const costTemplateSchema = new mongoose.Schema(
    {
        name: {
            type: String,
            name: "Cost Template Name",
            required: true,
            minLength: 2,
        },
        items: [
            {
                name: {
                    type: String,
                    name: "Item Name",
                    required: true,
                    minLength: 2,
                },
                type: {
                    type: String,
                    name: "Item Type",
                    enum: ["fixed", "variable", "hourly"],
                    required: true,
                },
                value: {
                    type: Number,
                    name: "Item Value",
                    required: true,
                    min: 0,
                },
                ledger: {
                    type: mongoose.Schema.Types.ObjectId,
                    ref: "LedgerHead", resolveBy: "name",
                    required: true,
                }
            }
        ],
        status: {
            type: String,
            name: "Status",
            enum: ["active", "inactive"],
            default: "active",
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
const CostTemplate = mongoose.model("CostTemplate", costTemplateSchema);
module.exports = CostTemplate;

```
