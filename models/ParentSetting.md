```javascript
// File: ./features/settings/parent/parent-setting.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const parentSettingSchema = new Schema(
    {
        fields: [FormFieldSchema],
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
        defaults: {
            session: {
                type: String,
                required: true,
                trim: true,
                minlength: 2,
                maxlength: 50,
                default: "{{currentYear}}-{{nextYear}}",
            },
        },
        accountCreationType: {
            type: String,
            enum: ["Father", "Mother", "Guardian"],
            default: "Father"
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
const ParentSetting = mongoose.model("ParentSetting", parentSettingSchema);
module.exports = ParentSetting;

```
