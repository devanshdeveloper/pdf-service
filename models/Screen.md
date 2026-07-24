```javascript
// File: ./features/screen/screen.model
const mongoose = require("mongoose");

const screenSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    description: {
        type: String,
        required: true
    },
    image: {
        type: String,
        required: true
    },
    location: {
        type: String,
        required: true
    },
    status: {
        type: String,
        enum: ["active", "inactive"],
        default: "active"
    },
    capacity: {
        type: Number,
        required: true
    },
    isDeleted: {
        type: Boolean,
        default: false
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        resolveBy: "username",
        required: true,
    },
    venue: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Venue",
        resolveBy: "name",
        required: true,
    }
}, { timestamps: true });

module.exports = mongoose.model("Screen", screenSchema);
```
