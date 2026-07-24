```javascript
// File: ./features/faq/faq.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const faqSchema = new Schema(
  {
    question: {
      type: String,
      required: true,
      trim: true,
      name: "Question"
    },
    answer: {
      type: String,
      required: true,
      trim: true,
      name: "Answer"
    },
    category: {
      type: String,
      required: true,
      trim: true,
      name: "Category"
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
      name: "Status"
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
      required: true
    }
  },
  { timestamps: true }
);
const FAQ = mongoose.model("FAQ", faqSchema);
module.exports = FAQ;

```
