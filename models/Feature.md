```javascript
// File: ./features/feature/feature.model
const mongoose = require("mongoose");
const featureSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      name: "Feature Name",
    },
    description: {
      type: String,
      trim: true,
      name: "Description",
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
      name: "Status",
    },
    icon: {
      type: String,
      trim: true,
      name: "Icon",
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
// Create the model
const Feature = mongoose.model("Feature", featureSchema);
module.exports = Feature;

```
