```javascript
// File: ./features/permission/permission.model
const mongoose = require("mongoose");
const PermissionHelper = require("./PermissionHelper");
const permissionSchema = new mongoose.Schema(
  {
    permissions: {
      type: mongoose.Schema.Types.Mixed,
      required: true,
      validate: {
        validator: function (permissions) {
          const isValid = PermissionHelper.validatePermissions(permissions);
          return isValid;
        },
        message:
          "Invalid permission format. Expected array of [ModuleName, Operations[], MonthlyLimit, YearlyLimit]",
      },
    },
  },
  {
    timestamps: true,
  }
);
const Permission = mongoose.model("Permission", permissionSchema);
module.exports = Permission;

```
