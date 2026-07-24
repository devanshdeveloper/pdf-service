```javascript
// File: ./features/show-timing/show-timing.model
const mongoose = require("mongoose");

const showTimingSchema = new mongoose.Schema({
    show: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Show",
        required: true,
    },
    screen: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Screen",
        required: true,
    },
    start: {
        type: Date,
        required: true
    },
    end: {
        type: Date,
        required: true
    },
    price: {
        type: Number,
        required: true
    },
    status: {
        type: String,
        enum: ["active", "inactive", "full", "cancelled"],
        default: "active"
    },
    isDeleted: {
        type: Boolean,
        default: false
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true,
    }
}, { timestamps: true });

module.exports = mongoose.model("ShowTiming", showTimingSchema);
```
