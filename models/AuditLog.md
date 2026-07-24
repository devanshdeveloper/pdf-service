```javascript
// File: ./features/audit-log/audit-log.model
const mongoose = require("mongoose");
const auditLogSchema = new mongoose.Schema({
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User"
    },
    action: {
        type: String,
        required: true
    },
    data: {
        type: mongoose.Schema.Types.Mixed,
        required: true
    },
    userAgent: {
        type: mongoose.Schema.Types.Mixed,
        required: true
    },
    ip: {
        type: String,
        required: true
    },
}, {
    timestamps: true
});
module.exports = mongoose.model("AuditLog", auditLogSchema);
```
