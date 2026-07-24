```javascript
// File: ./features/form/form.model
const mongoose = require("mongoose");
const FormStatus = require("./constants/FromStatus");
const { Schema } = mongoose;
const FormFieldSchema = new Schema({
  label: {
    type: String,
    required: true,
    trim: true,
    minlength: 2,
    maxlength: 100,
  },
  type: {
    type: String,
    required: true,
  },
  required: {
    type: Boolean,
    default: false,
  },
  props: [
    {
      name: {
        type: String,
        required: true,
      },
      value: {
        type: Schema.Types.Mixed,
      },
    },
  ],
  validators: [
    {
      name: {
        type: String,
        required: true,
      },
      value: {
        type: Schema.Types.Mixed,
      },
    },
  ],
});
const FormSchema = new Schema(
  {
    title: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 100,
    },
    description: {
      type: String,
      trim: true,
      maxlength: 500,
    },
    fields: [FormFieldSchema],
    status: {
      type: String,
      enum: Object.values(FormStatus),
      default: "draft",
    },
    model: {
      type: String,
      trim: true,
      minlength: 2,
      maxlength: 100,
    },
    isPublic: {
      type: Boolean,
      default: false,
    },
    userTypes: {
      type: [{ type: String }],
    },
    maxSubmitCount: {
      type: Number,
      default: 1,
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
const Form = mongoose.model("Form", FormSchema);
module.exports = Form;
module.exports.FormFieldSchema = FormFieldSchema;

```
