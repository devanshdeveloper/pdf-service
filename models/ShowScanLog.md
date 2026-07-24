```javascript
// File: ./features/show-scan-log/show-scan-log.model
const mongoose = require("mongoose");

const showScanLogSchema = new mongoose.Schema({
    ticket: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowTicket",
        required: true
    },

    scanner: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowScanner",
        required: true
    },

    venue: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Venue"
    },

    scanTime: {
        type: Date,
        default: Date.now
    },

    status: {
        type: String,
        enum: ["success", "failed", "duplicate", "expired"],
        required: true
    },

    reason: String,

    gateAction: {
        type: String,
        enum: ["opened", "denied"]
    },

    rawQR: String,

});

module.exports = mongoose.model("ShowScanLog", showScanLogSchema);      
```
