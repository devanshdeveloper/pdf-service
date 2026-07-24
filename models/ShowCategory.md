```javascript
// File: ./features/show-category/show-category.model
const mongoose = require("mongoose");

const showCategorySchema = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    description: {
        type: String,
        required: true
    },
    image: {
        type: String,
        required: true
    },
    status: {
        type: String,
        enum: ["active", "inactive"],
        default: "active"
    },
    isDeleted: {
        type: Boolean,
        default: false
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        resolveBy: "username",
        required: true,
    }
}, { timestamps: true });

module.exports = mongoose.model("ShowCategory", showCategorySchema);
```
