```javascript
// File: ./features/settings/teacher/teacher-settings.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const teacherSettingSchema = new Schema(
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
const TeacherSettings = mongoose.model("TeacherSettings", teacherSettingSchema);
module.exports = TeacherSettings;

```
