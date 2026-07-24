```javascript
// File: ./features/ai/base-prompt-category/base-prompt-category.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const BasePromptCategorySchema = new Schema(
    {
        parent: {
            type: Schema.Types.ObjectId,
            ref: "BasePromptCategory",
            resolveBy: "name",
            name: "Parent Category"
        },
        name: {
            type: String,
            required: true,
            trim: true,
            name: "Category Name"
        },
        image: {
            type: String,
            trim: true,
            name: "Category Image"
        },
        description: {
            type: String,
            trim: true,
            name: "Category Description"
        },
        slug: {
            type: String,
            trim: true,
            lowercase: true,
            name: "URL Slug"
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
            name: "User"
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted"
        },
    }, {
    timestamps: true
}
);
const BasePromptCategory = mongoose.model("BasePromptCategory", BasePromptCategorySchema);
module.exports = BasePromptCategory;

```
