```javascript
// File: ./features/salary/salary.model
const mongoose = require("mongoose");
const salarySchema = new mongoose.Schema(
    {
        name: {
            type: String,
            name: "Name",
            required: true,
        },
        income: [
            {
                name: {
                    type: String,
                    name: "Income Name",
                    required: true,
                },
                amount: {
                    type: Number,
                    name: "Amount",
                    required: true,
                    min: 0,
                },
            }
        ],
        deduction: [
            {
                name: {
                    type: String,
                    name: "Deduction Name",
                    required: true,
                },
                amount: {
                    type: Number,
                    name: "Amount",
                    required: true,
                    min: 0,
                },
            }
        ],
        earnings: {
            type: Number,
            name: "Earnings",
            required: true,
            min: 0,
        },
        deductions: {
            type: Number,
            name: "Deductions",
            required: true,
            min: 0,
        },
        net: {
            type: Number,
            name: "Net Payable",
            required: true,
            min: 0,
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
    },
    { timestamps: true }
);
module.exports = mongoose.model("Salary", salarySchema);
```
