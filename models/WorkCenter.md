```javascript
// File: ./features/work-center/work-center.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const mapItemSchema = new Schema({
    product: { type: Schema.Types.ObjectId, ref: 'Item' }, // null/omitted => "All Items"
    appliesToAllItems: { type: Boolean, default: true },
    setupTimeSeconds: { type: Number, required: true, min: 0 },   // e.g. 360*60
    cleanupTimeSeconds: { type: Number, required: true, min: 0 },
    capacity: { type: Number, required: true, min: 1 },
}, { _id: false });

const workCenterSchema = new Schema({
    name: { type: String, required: true, trim: true }, // "Engine Assembly Station"
    workCenterType: { type: Schema.Types.ObjectId, ref: 'WorkCenterType', required: true },
    // Optional override — if absent, inherit costTemplate from workCenterType
    costTemplate: { type: Schema.Types.ObjectId, ref: 'CostTemplate' },
    workCenterTiming: { type: Schema.Types.ObjectId, ref: 'WorkCenterTiming', required: true },
    mapItems: {
        type: [mapItemSchema],
        validate: v => Array.isArray(v) && v.length > 0,
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
}, { timestamps: true });
const WorkCenter = mongoose.model("WorkCenter", workCenterSchema);
module.exports = WorkCenter;

```
