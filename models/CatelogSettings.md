```javascript
// File: ./features/settings/catelog/catelog-settings.model
const mongoose = require("mongoose");
const CatelogSettingsSchema = new mongoose.Schema(
    {
        catelog: {
            type: Boolean,
            default: false
        },
        isPublic: {
            type: Boolean,
            default: false
        },
        showStock: {
            type: Boolean,
            default: false
        },
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
    },
    { timestamps: true }
);
module.exports = mongoose.model("CatelogSettings", CatelogSettingsSchema);
```
