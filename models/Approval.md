```javascript
// File: ./features/approval/approval.model
const mongoose = require("mongoose");
const approvalSchema = new mongoose.Schema(
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
module.exports = mongoose.model("Approval", approvalSchema);
```
