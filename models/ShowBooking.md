```javascript
// File: ./features/show-booking/show-booking.model
const mongoose = require("mongoose");
const { ShowBookingPaymentStatusTypesArray, ShowBookingPaymentStatusTypes } = require("./show-booking");
const PaymentMethods = require("../../data/PaymentMethods");

const showBookingSchema = new mongoose.Schema({
    showTiming: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "ShowTiming",
        required: true,
    },
    totalAmount: {
        type: Number,
        required: true,
    },
    paymentStatus: {
        type: String,
        enum: ShowBookingPaymentStatusTypesArray,
        default: ShowBookingPaymentStatusTypes.Sent,
    },
    paymentMethod: {
        type: String,
        enum: Object.values(PaymentMethods),
        default: PaymentMethods.Cash,
    },
    paidDate: {
        type: Date,
    },
    customer: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "Customer",
        required: true,
    },
    // customer id
    user_id: {
        type: String,
        required: true,
    },
    bookingSource: {
        type: String,
        enum: ["web", "pos", "android"],
        default: "web",
    },
    // admin id
    createdBy: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true,
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User",
        required: true,
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
}, { timestamps: true });

module.exports = mongoose.model("ShowBooking", showBookingSchema);
```
