```javascript
// File: ./features/settings/party/party-settings.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const customerSettingSchema = new Schema(
    {
        templates: {
            username: {
                type: String,
                required: true,
                trim: true,
                minlength: 2,
                maxlength: 50,
                default: "{{firstName}}{{lastName}}",
            },
            password: {
                type: String,
                required: true,
                trim: true,
                minlength: 2,
                maxlength: 50,
                default: "{{firstName}}{{birthYear}}",
            },
        },
        fields: [FormFieldSchema],
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
            default: false,
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
const PartySetting = mongoose.model("PartySetting", customerSettingSchema);
module.exports = PartySetting;

```
