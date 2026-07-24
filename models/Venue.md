```javascript
// File: ./features/venue/venue.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");

const venueSchema = new mongoose.Schema({
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
    address: addressSchema,
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

module.exports = mongoose.model("Venue", venueSchema);
```
