```javascript
// File: ./features/product-field/product-field.model
const mongoose = require("mongoose");
const { Schema } = mongoose;

const productFieldSchema = new Schema(
    {
        category: {
            type: Schema.Types.ObjectId,
            ref: "Category",
            resolveBy: "name",
            required: true,
            name: "Category",
        },
        fields: {
            type: [Schema.Types.Mixed],
            name: "Fields",
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User",
            resolveBy: "username",
            required: true,
            name: "User"
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted",
        },
    },
    {
        timestamps: true,
    }
);

const ProductField = mongoose.model("ProductField", productFieldSchema);
module.exports = ProductField;

```
