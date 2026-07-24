```javascript
// File: ./features/ledger/ledger.model
const mongoose = require('mongoose');
const PaymentMethods = require('../../data/PaymentMethods');
const { Schema } = mongoose;
// ========== TransactionType Enum ==========
const TransactionType = Object.freeze({
    CREDIT: 'credit',
    DEBIT: 'debit'
});
// ========== LedgerEntry Schema ==========
const ledgerEntrySchema = new Schema({
    type: {
        type: String,
        required: true,
        enum: Object.values(TransactionType),
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
    attachments: [{
        type: String,
        trim: true
    }],
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
ledgerEntrySchema.statics.TransactionType = TransactionType;
module.exports = mongoose.model('LedgerEntry', ledgerEntrySchema);

```
