```javascript
// File: ./features/form/form-response.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const FormResponseSchema = new Schema(
  {
    form: {
      type: Schema.Types.ObjectId,
      ref: "Form",
      required: true,
    },
    responses: [
      {
        field: {
          type: String,
          required: true,
        },
        value: {
          type: Schema.Types.Mixed,
          required: true,
        },
      },
    ],
    status: {
      type: String,
      enum: ["pending", "submitted", "rejected"],
      default: "submitted",
    },
    ip: {
      type: String,
    },
    userId: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    isDeleted: {
      type: Boolean,
      default: false,
    },
  },
  {
    timestamps: true,
  }
);
const FormResponse = mongoose.model("FormResponse", FormResponseSchema);
module.exports = FormResponse;

```
