```javascript
// File: ./features/stream/stream.model
const { Schema, model } = require("mongoose");
const streamSchema = new Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      name: "Stream Name",
    },
    description: {
      type: String,
      trim: true,
      name: "Description",
    },
    code: {
      type: String,
      uppercase: true,
      name: "Code",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      name: "User"
    }
  },
  {
    timestamps: true,
  }
);
module.exports = model("Stream", streamSchema);

```
