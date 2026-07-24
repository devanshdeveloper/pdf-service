```javascript
// File: ./features/reimbursement/reimbursement.model
const mongoose = require("mongoose");
const reimbursementSchema = new mongoose.Schema(
    {
        title: {
            type: String,
            required: true,
            trim: true,
        },
        description: {
            type: String,
            trim: true,
        },
        amount: {
            type: Number,
            required: true,
            min: 0,
        },
        status: {
            type: String,
            enum: ["draft", "pending", "approved", "rejected"],
            default: "pending",
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
        approvedBy: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
        },
        rejectedBy: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
        },
        attachments: [{
            type: String,
            trim: true
        }],
        requestedBy: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
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
module.exports = mongoose.model("Reimbursement", reimbursementSchema);
```
