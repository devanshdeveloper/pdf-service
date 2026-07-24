```javascript
// File: ./features/category/category.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const CategorySchema = new Schema(
  {
    parent: {
      type: Schema.Types.ObjectId,
      ref: "Category",
      resolveBy: "name",
      name: "Parent Category"
    },
    name: {
      type: String,
      required: true,
      trim: true,
      name: "Category Name"
    },
    cover_image: {
      type: String,
      trim: true,
      name: "Cover Image"
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
    featured: {
      type: Boolean,
      default: false,
      name: "Featured",
    },
    slug: {
      type: String,
      trim: true,
      lowercase: true,
      name: "URL Slug"
    },
    sort_priority: {
      type: Number,
      default: 0,
      name: "Sort Priority"
    },
    fields: {
      type: [
        {
          label: {
            type: String,
            required: true,
          },
          value: {
            type: Schema.Types.Mixed,
            required: true,
          },
        },
      ],
      name: "Custom Fields",
    },
    hidden: {
      type: Boolean,
      default: false,
      name: "Is Hidden"
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
      name: "Status"
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
const Category = mongoose.model("Category", CategorySchema);
module.exports = Category;

```
