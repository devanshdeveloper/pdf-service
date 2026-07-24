```javascript
// File: ./features/documentation/documentation.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const documentationSchema = new Schema(
  {
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
      enum: ["draft", "published"],
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
    }
  },
  {
    timestamps: true,
  }
);
const Documentation = mongoose.model("Documentation", documentationSchema);
module.exports = Documentation;
```
