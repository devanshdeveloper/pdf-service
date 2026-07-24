```javascript
// File: ./features/visitor/visitor.model
const mongoose = require("mongoose");
const visitorSchema = new mongoose.Schema({
    ip: {
        type: String,
    },
    device_id: {
        type: String,
    },
    utm: {
        source: {
            type: String,
        },
        medium: {
            type: String,
        },
        campaign: {
            type: String,
        },
        term: {
            type: String,
        },
        content: {
            type: String,
        }
    },
    first_page: {
        type: String,
    },
    merged_user_id: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true
    }
}, { timestamps: true })
module.exports = mongoose.model("Visitor", visitorSchema);
```
