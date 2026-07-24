```javascript
// File: ./features/settings/bill-of-material-line-item-field/bill-of-material-line-item-fields.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const billOfMaterialLineItemFieldSchema = new Schema(
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
const BillOfMaterialLineItemField = mongoose.model("BillOfMaterialLineItemField", billOfMaterialLineItemFieldSchema);
module.exports = BillOfMaterialLineItemField;

```
