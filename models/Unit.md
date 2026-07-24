```javascript
// File: ./features/unit/unit.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const UnitSchema = new Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      name: "Unit Name",
    },
    value: {
      type: String,
      required: true,
      trim: true,
      name: "Unit Value",
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
      name: "Status",
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true
    },
    baseUnit: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Unit",
      resolveBy: "name",
      name: "Base Unit",
    },
    conversionFactor: {
      type: Number,
      name: "Conversion Factor (multiplier to get baseUnit)",
      default: 1,
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
    },
  },
  { timestamps: true }
);
const Unit = mongoose.model("Unit", UnitSchema);
module.exports = Unit;

```
