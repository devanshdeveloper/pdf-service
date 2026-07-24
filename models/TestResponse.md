```javascript
// File: ./features/test/test-response.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const TestResponseSchema = new Schema(
  {
    test: {
      type: Schema.Types.ObjectId,
      ref: "Test",
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
const TestResponse = mongoose.model("TestResponse", TestResponseSchema);
module.exports = TestResponse;

```
