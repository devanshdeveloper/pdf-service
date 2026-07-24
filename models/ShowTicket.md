```javascript
// File: ./features/show-ticket/show-ticket.model
const mongoose = require("mongoose");

const ShowTicketStatusTypesArray = ["VALID", "USED", "CANCELLED"];
const ShowTicketStatusTypes = {
    Valid: "VALID",
    Used: "USED",
    Cancelled: "CANCELLED",
};

const showTicketSchema = new mongoose.Schema({
    booking: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowBooking",
        required: true,
    },
    showTiming: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowTiming",
        required: true,
    },
    ticketNumber: {
        type: String,
        required: true,
    },
    status: {
        type: String,
        enum: ShowTicketStatusTypesArray,
        default: ShowTicketStatusTypes.Valid,
    },
    price: {
        type: Number,
        required: true,
    },
    createdAt: {
        type: Date,
        default: Date.now,
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
    customer: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Customer",
        required: true,
    },
    user_id: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true,
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true,
    }
}, { timestamps: true });

module.exports = mongoose.model("ShowTicket", showTicketSchema);
```
