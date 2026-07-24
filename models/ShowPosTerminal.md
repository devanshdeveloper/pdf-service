```javascript
// File: ./features/show-pos-terminal/show-pos-terminal.model
const mongoose = require("mongoose");

const showPosTerminalSchema = new mongoose.Schema({
    name: { type: String, required: true },

    venue: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Venue",
        required: true
    },
    pos_id: {
        type: String,
        required: true
    },
    location: {
        type: String // e.g. "Counter 1", "Lobby"
    },
    userId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true
    },
    status: {
        type: String,
        enum: ["active", "inactive"],
        default: "active"
    },
    isDeleted: { type: Boolean, default: false },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true
    }
}, { timestamps: true });

module.exports = mongoose.model("ShowPosTerminal", showPosTerminalSchema);
```
