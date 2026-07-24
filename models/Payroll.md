```javascript
// File: ./features/payroll/payroll.model
const mongoose = require("mongoose");
const DateHelper = require("../../helpers/DateHelper");
const payrollSchema = new mongoose.Schema(
    {
        employee: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Employee",
            required: true,
            name: "Employee",
        },
        start: {
            type: Date,
            name: "Start Date",
            required: true
        },
        end: {
            type: Date,
            name: "End Date",
            required: true
        },
        paidDays: {
            type: Number,
            name: "Paid Days",
            required: true,
            min: 0,
        },
        lossDays: {
            type: Number,
            name: "Loss Days",
            required: true,
            min: 0,
        },
        paymentDate: {
            type: Date,
            name: "Payment Date",
            required: true
        },
        month: {
            type: String,
            name: "Month",
            required: true,
            enum: DateHelper.MonthsNameArray
        },
        year: {
            type: Number,
            name: "Year",
            required: true,
            min: 0,
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
module.exports = mongoose.model("Payroll", payrollSchema);
```
