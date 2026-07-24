```javascript
// File: ./features/payment-transaction/payment-transaction.model
const mongoose = require('mongoose');
const PaymentMethods = require('../../data/PaymentMethods');
const { Schema } = mongoose;
const PaymentTransactionType = Object.freeze({
    SENT: 'sent',
    RECEIVED: 'received'
});
// ========== LedgerEntry Schema ==========
const PaymentTransactionSchema = new Schema({
    type: {
        type: String,
        required: true,
        enum: Object.values(PaymentTransactionType),
    },
    head: {
        type: Schema.Types.ObjectId,
        ref: 'LedgerHead',
        required: true,
    },
    amount: {
        type: Number,
        required: true,
    },
    mode: {
        type: String,
        required: true,
        enum: Object.values(PaymentMethods),
    },
    remark: {
        type: String,
        trim: true
    },
    date: {
        type: Date,
        required: true,
        default: Date.now
    },
    party: {
        type: Schema.Types.ObjectId,
        ref: 'Party', resolveBy: "businessName",
        required: true,
    },
    voucher: {
        type: Schema.Types.ObjectId,
        ref: 'Voucher', resolveBy: "number",
        required: true,
    },
    isDeleted: {
        type: Boolean,
        default: false,
    },
    user: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true,
    },
}, {
    timestamps: true
});
// attach enums to statics
PaymentTransactionSchema.statics.PaymentTransactionType = PaymentTransactionType;
module.exports = mongoose.model('PaymentTransaction', PaymentTransactionSchema);

```
