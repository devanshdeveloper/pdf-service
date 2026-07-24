```javascript
// File: ./features/settings/ai/ai-settings.model
const mongoose = require("mongoose");
const AISettingsSchema = new mongoose.Schema(
    {
        provider: {
            type: String,
            required: true,
        },
        apiKey: {
            type: String,
        },
        status: {
            type: String,
            enum: ["active", "inactive"],
            default: "active",
        },
        model: {
            type: String,
            required: true,
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
    },
    { timestamps: true }
);
module.exports = mongoose.model("AISettings", AISettingsSchema);
```
