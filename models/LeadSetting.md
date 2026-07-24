```javascript
// File: ./features/settings/lead/lead-settings.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const leadSettingSchema = new Schema(
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
        fields: [FormFieldSchema],
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
const LeadSetting = mongoose.model("LeadSetting", leadSettingSchema);
module.exports = LeadSetting;

```
