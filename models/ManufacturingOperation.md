```javascript
// File: ./features/manufacturing-operation/manufacturing-operation.model
const mongoose = require("mongoose");
const manufacturingOperationSchema = new mongoose.Schema(
    {
        name: {
            type: String,
            name: "Manufacturing Operation Name",
            required: true,
            minLength: 2,
        },
        description: {
            type: String,
            name: "Manufacturing Operation Description",
            required: true,
            minLength: 2,
        },
        workCenterType: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "WorkCenterType", resolveBy: "name",
            required: true,
        },
        attachments: [{
            type: String,
            trim: true
        }],
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
const ManufacturingOperation = mongoose.model("ManufacturingOperation", manufacturingOperationSchema);
module.exports = ManufacturingOperation;

```
