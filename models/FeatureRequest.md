```javascript
// File: ./features/feature-request/feature-request.model
const mongoose = require("mongoose");
const featureRequestSchema = new mongoose.Schema(
  {
    title: {
      type: String,
      required: true,
      minLength: 2,
      name: "Title",
    },
    description: {
      type: String,
      name: "Description",
    },
    upvotes: {
      type: Number,
      default: 0,
    },
    application: {
      type: String,
    },
    status: {
      type: String,
      enum: ["open", "in_progress", "testing", "resolved", "closed"],
      default: "open",
      name: "Status",
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "User",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Deleted Status",
    },
  },
  { timestamps: true, strictPopulate: false }
);
const FeatureRequest = mongoose.model("FeatureRequest", featureRequestSchema);
module.exports = FeatureRequest;

```
