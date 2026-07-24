```javascript
// File: ./features/settings/super-admin-lead/super-admin-lead-setting.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const superAdminLeadSettingSchema = new Schema(
    {
        identifiers: {
            type: [
                {
                    label: {
                        type: String,
                        required: true,
                        trim: true,
                        minlength: 2,
                        maxlength: 50,
                    },
                    prefix: {
                        type: String,
                        trim: true,
                        maxlength: 100,
                    },
                    counter: {
                        type: Number,
                        default: 1001,
                        min: 1,
                    },
                    template: {
                        type: String,
                        trim: true,
                        maxlength: 100,
                    }
                },
            ],
        },
        fields: [FormFieldSchema],
        templates: {
            email: {
                type: String,
                trim: true,
                minlength: 2,
                maxlength: 50,
                default: "{{email}}",
            },
            password: {
                type: String,
                trim: true,
                minlength: 2,
                maxlength: 50,
                default: "{{firstName}}{{birthYear}}",
            },
        },
        documents: {
            type: [
                {
                    label: {
                        type: String,
                        trim: true,
                        maxlength: 50,
                    },
                    required: {
                        type: Boolean,
                        default: false,
                    },
                },
            ],
        },
        accountCreationEnabled: {
            type: Boolean,
            default: true,
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
const SuperAdminLeadSetting = mongoose.model("SuperAdminLeadSetting", superAdminLeadSettingSchema);
module.exports = SuperAdminLeadSetting;

```
