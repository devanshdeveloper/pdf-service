```javascript
// File: ./features/show-seat-template/show-seat-template.model
const mongoose = require("mongoose");

const showSeatTemplateSchema = new mongoose.Schema({
    screen: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Screen",
    },
    section: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowSeatSection",
    },
    row: {
        type: String,
        required: true,
    },
    number: {
        type: Number,
        required: true,
    },
    seatLabel: {
        type: String,
        required: true,
    },
    price: {
        type: Number,
        required: false,
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
        required: true
    }
}, { timestamps: true });

module.exports = mongoose.model("ShowSeatTemplate", showSeatTemplateSchema);
```
