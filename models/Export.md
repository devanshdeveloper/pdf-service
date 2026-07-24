```javascript
// File: ./features/export/export.model
const mongoose = require("mongoose");
const exportSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true,
    },
    type: {
        type: String,
        enum: ["csv", "excel", "pdf"],
        required: true,
    },
    model: {
        type: String,
        required: true,
    },
    selectedFields: {
        type: [String],
        required: true,
    },
    lockedAt: {
        type: Date,
        default: null,
    },
    lockedBy: {
        type: String,
        default: null,
    },
    url: {
        type: String,
        default: null,
    },
    result: {
        success: {
            type: Number,
            default: 0,
        },
        errors: {
            type: Number,
            default: 0,
        },
    },
    filter: {
        type: mongoose.Schema.Types.Mixed,
        default: {},
    },
    config: {
        type: mongoose.Schema.Types.Mixed,
        default: {},
    },
    status: {
        type: String,
        enum: ["pending", "processing", "completed", "failed"],
        default: "pending",
    },
    error: {
        type: String,
        default: null,
    },
    isDeleted: {
        type: Boolean,
        default: false,
    },
    errors: {
        type: mongoose.Schema.Types.Mixed,
        default: {},
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true,
    }
}, { timestamps: true });
const Export = mongoose.model("Export", exportSchema);
module.exports = Export;

```
