```javascript
// File: ./features/signature/signature.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const signatureSchema = new Schema(
    {
        name: {
            type: String,
            required: true,
            trim: true,
            minlength: 2,
            maxlength: 100,
            name: "Signature Name"
        },
        url: {
            type: String,
            trim: true,
        },
        status: {
            type: String,
            enum: ["active", "inactive"],
            default: "active",
        },
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
    },
    { timestamps: true }
);
const Signature = mongoose.model("Signature", signatureSchema);
module.exports = Signature;

```
