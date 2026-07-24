```javascript
// File: ./features/settings/category/category-setting.model
const mongoose = require("mongoose");
const { FormFieldSchema } = require("../../form/form.model");
const { Schema } = mongoose;
const categorySettingSchema = new Schema(
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
const CategorySetting = mongoose.model("CategorySetting", categorySettingSchema);
module.exports = CategorySetting;

```
