```javascript
// File: ./features/classroom/classroom.model
const mongoose = require("mongoose");
const ClassroomSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 50,
    },
    roomNumber: {
      type: String,
      required: true,
      trim: true,
    },
    location: {
      type: String,
      required: true,
      trim: true,
    },
    capacity: {
      type: Number,
      required: true,
      min: 1,
    },
    resources: [
      {
        type: String,
      },
    ],
    status: {
      type: String,
      enum: ["active", "maintenance", "inactive"],
      default: "active",
    },
    isDeleted: {
      type: Boolean,
      default: false,
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model("Classroom", ClassroomSchema);

```
