```javascript
// File: ./features/blog/blog.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const blogSchema = new Schema(
    {
        cover_image: {
            type: String,
            name: "Cover Image"
        },

        image: {
            type: String,
            name: "Image"
        },
        title: {
            type: String,
            required: true,
            trim: true,
            minlength: 2,
            name: "Title"
        },
        slug: {
            type: String,
            required: true,
            unique: true,
            trim: true,
            lowercase: true,
            name: "URL Slug"
        },
        content: {
            type: String,
            required: true,
            name: "Content"
        },
        status: {
            type: String,
            enum: ["draft", "published", "archived"],
            default: "draft",
            name: "Status"
        },
        order: {
            type: Number,
            default: 0,
            name: "Display Order"
        },
        metadata: {
            type: Map,
            of: Schema.Types.Mixed,
            default: new Map(),
            name: "Additional Metadata"
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted"
        },
        author_name: {
            type: String,
            name: "Author Name"
        },
        author_image: {
            type: String,
            name: "Author Image"
        },
        author_id: {
            type: Schema.Types.ObjectId,
            ref: "User",
            name: "Author ID"
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User",
            name: "User"
        }
    },
    {
        timestamps: true,
    }
);
const Blog = mongoose.model("Blog", blogSchema);
module.exports = Blog;
```
