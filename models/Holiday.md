```javascript
// File: ./features/holiday/holiday.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
// ========== Holiday Model ==========
// HolidayStatus enum
const HolidayStatus = Object.freeze({
    DRAFT: "draft",
    PENDING: "pending",
    APPROVED: "approved",
    REJECTED: "rejected"
});
// HolidayType enum
const HolidayType = Object.freeze({
    PUBLIC: "public",
    PRIVATE: "private"
});
const holidaySchema = new Schema(
    {
        name: {
            type: String,
            required: true,
        },
        description: {
            type: String,
            trim: true,
        },
        start: {
            type: Date,
            required: true,
        },
        end: {
            type: Date,
            required: true,
        },
        allDay: {
            type: Boolean,
            default: false,
        },
        type: {
            type: String,
            required: true,
            enum: Object.values(HolidayType),
            default: HolidayType.PUBLIC,
        },
        status: {
            type: String,
            required: true,
            enum: Object.values(HolidayStatus),
            default: HolidayStatus.DRAFT,
        },
        remark: {
            type: String,
            trim: true,
        },
        recordedBy: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
        },
        isDeleted: {
            type: Boolean,
            default: false,
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
holidaySchema.statics.HolidayStatus = HolidayStatus;
module.exports = mongoose.model("Holiday", holidaySchema);

```
