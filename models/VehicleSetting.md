```javascript
// File: ./features/settings/vehicle/vehicle-setting.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const vehicleSettingSchema = new Schema(
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
        types: {
            type: [String],
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
const VehicleSetting = mongoose.model("VehicleSetting", vehicleSettingSchema);
module.exports = VehicleSetting;

```
