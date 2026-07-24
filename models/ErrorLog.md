```javascript
// File: ./features/error-log/error-log.model
const mongoose = require("mongoose");
const errorLogSchema = new mongoose.Schema({
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User"
    },
    name: {
        type: String,
    },
    stack: {
        type: String,
    },
    message: {
        type: String,
    },
    code: {
        type: String,
    },
    field: {
        type: String,
    },
    module: {
        type: String,
    },
    status: {
        type: String,
        enum: ["pending", "resolved", "closed"],
        default: "pending"
    },
    userAgent: {
        type: mongoose.Schema.Types.Mixed,
    },
    ip: {
        type: String,
    }
}, {
    timestamps: true
});
module.exports = mongoose.model("ErrorLog", errorLogSchema);
```
