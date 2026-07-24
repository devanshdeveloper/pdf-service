```javascript
// File: ./features/settings/time-table/time-table-settings.model
const mongoose = require('mongoose');
const { Schema } = mongoose;
const TimetableSettingsSchema = new Schema({
    name: {
        type: String,
        required: true,
        trim: true,               // e.g. “Default Term Settings” or “Spring 2025”
    },
    workingDays: {
        type: [String],
        enum: [
            'Monday',
            'Tuesday',
            'Wednesday',
            'Thursday',
            'Friday',
            'Saturday',
            'Sunday'
        ],
        default: [
            'Monday',
            'Tuesday',
            'Wednesday',
            'Thursday',
            'Friday'
        ],
        validate: {
            validator: arr => arr.length > 0,
            message: 'There must be at least one working day'
        }
    },
    dayStart: {
        type: Number,
        required: true,
        min: 0,
        max: 24 * 60 - 1,         // e.g. 8:00 AM = 480
    },
    dayEnd: {
        type: Number,
        required: true,
        min: 1,
        max: 24 * 60,             // e.g. 6:00 PM = 1080
    },
    slotDuration: {
        type: Number,
        required: true,
        min: 1,
        max: 24 * 60
    },
    breakDuration: {
        type: Number,
        default: 0,
        min: 0,
        max: 24 * 60
    },
    status: {
        type: String,
        enum: ['active', 'inactive'],
        default: 'inactive',
        required: true
    },
    user: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true,
    },
    isDeleted: {
        type: Boolean,
        default: false
    }
}, {
    timestamps: true
});
module.exports = mongoose.model('TimetableSettings', TimetableSettingsSchema);

```
