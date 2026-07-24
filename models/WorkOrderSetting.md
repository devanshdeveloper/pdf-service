```javascript
// File: ./features/settings/work-order/work-order-setting.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const workOrderSettingSchema = new Schema(
    {
        prefix: {
            type: String,
            trim: true,
            maxlength: 100,
        },
        counter: {
            type: Number,
            default: 1001,
            min: 1,
        },
        template: {
            type: String,
            trim: true,
            maxlength: 100,
        },
        types: {
            type: [String],
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
    },
    {
        timestamps: true,
    }
);
const WorkOrderSetting = mongoose.model("WorkOrderSetting", workOrderSettingSchema);
module.exports = WorkOrderSetting;

```
