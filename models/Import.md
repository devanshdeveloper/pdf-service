```javascript
// File: ./features/import/import.model
const mongoose = require("mongoose");
const importSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true,
    },
    model: {
        type: String,
        required: true,
    },
    operation: {
        type: String,
        enum: [
            "insert_only",
            "update_only",
            "upsert"
        ],
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
    status: {
        type: String,
        enum: ["pending", "uploading", "processing", "validating", "importing", "completed", "completed_with_errors", "failed"],
        default: "pending",
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
    error: {
        type: String,
        default: null,
    },
    errors: [{
        type: mongoose.Schema.Types.Mixed,
        default: null,
    }],
    isDeleted: {
        type: Boolean,
        default: false,
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true,
    }
}, { timestamps: true });
const Import = mongoose.model("Import", importSchema);
module.exports = Import;

```
