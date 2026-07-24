```javascript
// File: ./features/show/show.model
const mongoose = require("mongoose");

const showSchema = new mongoose.Schema({
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
    duration: {
        type: Number,
        required: true
    },
    language: {
        type: String,
        required: true
    },
    showCategory: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowCategory",
        resolveBy: "name",
        required: true,
    },
    status: {
        type: String,
        enum: ["active", "inactive"],
        default: "active"
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
    }
}, { timestamps: true });

module.exports = mongoose.model("Show", showSchema);
```
