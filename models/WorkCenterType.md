```javascript
// File: ./features/work-center-type/work-center-type.model
const mongoose = require("mongoose");
const workCenterTypeSchema = new mongoose.Schema(
    {
        name: {
            type: String,
            name: "Cost Name",
            required: true,
            minLength: 2,
        },
        description: {
            type: String,
            name: "Description",
            required: false,
        },
        costTemplate: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "CostTemplate", resolveBy: "name",
            required: true,
        },
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
const WorkCenterType = mongoose.model("WorkCenterType", workCenterTypeSchema);
module.exports = WorkCenterType;

```
