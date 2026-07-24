```javascript
// File: ./features/cost/cost.model
const mongoose = require("mongoose");
const costSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      name: "Cost Name",
      required: true,
      minLength: 2,
    },
    type: {
      type: String,
      name: "Cost Type",
      enum: ["fixed", "variable"],
      required: true,
    },
    value: {
      type: Number,
      name: "Cost Value",
      required: true,
      min: 0,
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    isDeleted: {
      type: Boolean,
      default: false,
    },
  },
  { timestamps: true }
);
const Cost = mongoose.model("Cost", costSchema);
module.exports = Cost;

```
