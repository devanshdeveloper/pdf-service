```javascript
// File: ./features/show-scanner/show-scanner.model
const mongoose = require("mongoose");

const showScannerSchema = new mongoose.Schema({
    name: { type: String, required: true },
    venue: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Venue",
        required: true
    },
    scanner_id: {
        type: String,
        required: true
    },
    location: {
        type: String // e.g. "Gate A", "Main Entrance"
    },
    userId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true
    },
    status: {
        type: String,
        enum: ["active", "inactive"],
        default: "active"
    },
    isDeleted: { type: Boolean, default: false },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true
    }
}, { timestamps: true });

module.exports = mongoose.model("ShowScanner", showScannerSchema);
```
