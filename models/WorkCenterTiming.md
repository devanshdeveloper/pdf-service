```javascript
// File: ./features/work-center-timing/work-center-timing.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const dayHoursSchema = new Schema({
    isWorkingDay: { type: Boolean, default: true },
    from: { type: String }, // "09:00"
    to: { type: String },   // "18:00"
}, { _id: false });

const breakSchema = new Schema({
    name: { type: String, required: true, trim: true }, // "Lunch break"
    from: { type: String, required: true },
    to: { type: String, required: true },
    isPaid: { type: Boolean, default: false },
}, { _id: false });

const workCenterTimingSchema = new Schema({
    name: { type: String, required: true, trim: true, unique: true }, // "Engine Assembly"
    configuration: {
        type: String,
        enum: ['standard', 'custom'], // standard = same hours every week, custom = per-weekday
        default: 'standard',
    },
    // Standard config: one set of hours applied to all selected weekdays
    standardHours: {
        from: { type: String, default: '09:00' },
        to: { type: String, default: '18:00' },
    },
    // Which weekdays are active (used in both standard + custom modes)
    workingDays: {
        sunday: { type: dayHoursSchema, default: () => ({}) },
        monday: { type: dayHoursSchema, default: () => ({}) },
        tuesday: { type: dayHoursSchema, default: () => ({}) },
        wednesday: { type: dayHoursSchema, default: () => ({}) },
        thursday: { type: dayHoursSchema, default: () => ({}) },
        friday: { type: dayHoursSchema, default: () => ({}) },
        saturday: { type: dayHoursSchema, default: () => ({}) },
    },
    breaks: { type: [breakSchema], default: [] },
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
const WorkCenterTiming = mongoose.model("WorkCenterTiming", workCenterTimingSchema);
module.exports = WorkCenterTiming;

```
