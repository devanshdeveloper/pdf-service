```javascript
// File: ./features/settings/product/product-setting.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const productSettingSchema = new Schema(
    {
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
const ProductSetting = mongoose.model("ProductSetting", productSettingSchema);
module.exports = ProductSetting;

```
