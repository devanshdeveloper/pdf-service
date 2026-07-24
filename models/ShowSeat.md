```javascript
// File: ./features/show-seat/show-seat.model
const mongoose = require("mongoose");

const showSeatSchema = new mongoose.Schema({
    seatTemplate: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowSeatTemplate",
    },
    show: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Show",
    },
    lockedBy: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
    },
    lockExpiresAt: {
        type: Date,
        index: true,
    },
    booking: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Booking",
    },
    price: {
        type: Number,
        required: true,
    },
    status: {
        type: String,
        enum: ["available", "booked", "blocked"],
        default: "available",
    },
    isDeleted: {
        type: Boolean,
        default: false,
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
    }

}, { timestamps: true });

module.exports = mongoose.model("ShowSeat", showSeatSchema);
```
