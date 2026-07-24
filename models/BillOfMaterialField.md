```javascript
// File: ./features/settings/bill-of-material-fields/bill-of-material-fields.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const billOfMaterialFieldsSchema = new Schema(
    {
        type: {
            type: String,
            required: true,
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
        status: {
            type: String,
            enum: ["draft", "active", "inactive"],
            default: "draft",
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
const BillOfMaterialFields = mongoose.model("BillOfMaterialFields", billOfMaterialFieldsSchema);
module.exports = BillOfMaterialFields;

```
