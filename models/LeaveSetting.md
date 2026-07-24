```javascript
// File: ./features/settings/leave/leave-settings.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const leaveSettingSchema = new Schema(
    {
        types: {
            type: [
                {
                    name: {
                        type: String,
                        required: true,
                        trim: true,
                    },
                    icon: {
                        type: String,
                        trim: true,
                    },
                    color: {
                        type: String,
                        required: true,
                        trim: true,
                    },
                }
            ],
            required: true
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
    },
    {
        timestamps: true,
    }
);
const LeaveSetting = mongoose.model("LeaveSetting", leaveSettingSchema);
module.exports = LeaveSetting;

```
