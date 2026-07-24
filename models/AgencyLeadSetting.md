```javascript
// File: ./features/settings/agency-lead/agency-lead-settings.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const agencyLeadSettingSchema = new Schema(
    {
        statuses: {
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
        sources: {
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
        channels: {
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
        stages: {
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
const AgencyLeadSetting = mongoose.model("AgencyLeadSetting", agencyLeadSettingSchema);
module.exports = AgencyLeadSetting;

```
