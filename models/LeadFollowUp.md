```javascript
// File: ./features/lead-follow-up/lead-follow-up.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const leadFollowUpSchema = new Schema(
  {
    lead: {
      type: Schema.Types.ObjectId,
      ref: "Lead",
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
      required: true,
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
const LeadFollowUp = mongoose.model("LeadFollowUp", leadFollowUpSchema);
module.exports = LeadFollowUp;
```
