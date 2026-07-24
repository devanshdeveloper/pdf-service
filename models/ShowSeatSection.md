```javascript
// File: ./features/show-seat-section/show-seat-section.model
const mongoose = require("mongoose");

const showSeatSectionSchema = new mongoose.Schema({
    screen: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Screen",
        resolveBy: "name",
    },
    name: {
        type: String,
        required: true,
    },
    price: {
        type: Number,
        required: true,
    },
    icon: {
        type: String,
    },
    color: {
        type: String,
    },
    order: {
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
        resolveBy: "username",
        required: true
    }
}, { timestamps: true });

module.exports = mongoose.model("ShowSeatSection", showSeatSectionSchema);
```
