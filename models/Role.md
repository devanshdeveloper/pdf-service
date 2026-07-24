```javascript
// File: ./features/role/role.model
const mongoose = require("mongoose");
const roleSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      required: true,
      unique: true,
      trim: true,
      name: 'Role Name'
    },
    permissions: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Permission",
      required: true,
      name: 'Permissions'
    },
    summary: {
      type: String,
      trim: true,
      name: 'Summary'
    },
    modules: [
      {
        type: String,
        ref: "Module",
        required: true,
        name: 'Modules'
      }
    ],
    description: {
      type: String,
      trim: true,
      name: 'Description'
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: 'Created By'
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model("Role", roleSchema);

```
