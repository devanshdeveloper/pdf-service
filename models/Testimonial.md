```javascript
// File: ./features/testimonial/testimonial.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const testimonialSchema = new Schema(
  {
    author: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 100,
      name: 'Author Name'
    },
    designation: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 100,
      name: 'Author Designation',
    },
    institution: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 100,
      name: 'Institution Name',
    },
    avatar: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      name: 'Avatar URL'
    },
    content: {
      type: String,
      required: true,
      trim: true,
      minlength: 10,
      maxlength: 1000,
      name: 'Testimonial Content'
    },
    rating: {
      type: Number,
      required: true,
      min: 1,
      max: 5,
      name: 'Rating'
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
      name: 'Status'
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: 'Is Deleted'
    },
  },
  {
    timestamps: true,
    toObject: { virtuals: true },
    toJSON: { virtuals: true }
  }
);
const Testimonial = mongoose.model("Testimonial", testimonialSchema);
module.exports = Testimonial;
```
