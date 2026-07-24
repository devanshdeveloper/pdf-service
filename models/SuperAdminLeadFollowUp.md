```javascript
// File: ./features/super-admin-lead-follow-up/super-admin-lead-follow-up.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const leadFollowUpSchema = new Schema(
    {
        lead: {
            type: Schema.Types.ObjectId,
            ref: "SuperAdminLead",
            required: true,
            name: "Lead"
        },
        scheduledAt: {
            type: Date,
            required: true,
            name: "Scheduled At"
        },
        status: {
            type: String,
            required: true,
            name: "Status",
        },
        remark: {
            type: String,
            trim: true,
            name: "Remark"
        },
        employee: {
            type: Schema.Types.ObjectId,
            ref: "Employee",
            name: "Employee"
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
            name: "User"
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted"
        }
    }, { timestamps: true }
);
const SuperAdminLeadFollowUp = mongoose.model("SuperAdminLeadFollowUp", leadFollowUpSchema);
module.exports = SuperAdminLeadFollowUp;
```
