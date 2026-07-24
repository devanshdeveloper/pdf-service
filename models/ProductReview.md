```javascript
// File: ./features/product-review/product-review.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const ProductReviewSchema = new Schema(
    {
        product: {
            type: Schema.Types.ObjectId,
            ref: "Product",
            required: true,
            name: "Product",
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User",
            required: true,
            name: "User",
        },
        rating: {
            type: Number,
            required: true,
            name: "Rating",
        },
        review: {
            type: String,
            required: true,
            name: "Review",
        },
        attachments: {
            type: [String],
            default: [],
            name: "Attachments",
        },
        status: {
            type: String,
            enum: ["pending", "approved", "rejected"],
            default: "pending",
            name: "Status",
        },
        createdBy: {
            type: Schema.Types.ObjectId,
            ref: "User",
            required: true,
            name: "Created By",
        },
        isDeleted: {
            type: Boolean,
            default: false,
            name: "Is Deleted",
        },
    },
    {
        timestamps: true,
    }
);
const ProductReview = mongoose.model("ProductReview", ProductReviewSchema);
module.exports = ProductReview;
```
