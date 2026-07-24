```javascript
// File: ./features/settings/district-operator/district-operator-setting.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const districtOperatorSettingSchema = new Schema(
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
                    },
                },
            ],
        },
        roles: {
            type: [String],
        },
        designations: {
            type: [String],
        },
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
        commission: {
            type: {
                type: String, // percentage | slab
                enum: ["slab", "percentage"],
                default: "slab",
            },
            percentage: {
                type: Number,
                default: 0,
            },
            defaultSlabs: [
                {
                    from: { type: Number },
                    to: { type: Number },
                    percentage: { type: Number },
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
const DistrictOperatorSettings = mongoose.model("DistrictOperatorSettings", districtOperatorSettingSchema);
module.exports = DistrictOperatorSettings;

```
