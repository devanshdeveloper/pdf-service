```javascript
// File: ./features/book-transaction/book-transaction.model
const mongoose = require('mongoose');
const Schema = mongoose.Schema;
const TransactionStatus = Object.freeze({
    BORROWED: 'borrowed',
    RETURNED: 'returned',
    OVERDUE: 'overdue',
    LOST: 'lost',
    RESERVED: 'reserved'
});
const borrowingTransactionSchema = new Schema({
    transactionId: { type: String, unique: true },
    borrowedBy: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true
    },
    book: {
        type: Schema.Types.ObjectId, ref: 'Book', resolveBy: "isbn", required: true,
    },
    borrowDate: {
        type: Date,
        default: Date.now,
    },
    dueDate: {
        type: Date, required: true
    },
    returnDate: {
        type: Date
    },
    status: {
        type: String,
        enum: Object.values(TransactionStatus),
        default: TransactionStatus.BORROWED,
    },
    isDeleted: { type: Boolean, default: false, },
    user: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true,
    },
}, { timestamps: true });
borrowingTransactionSchema.statics.TransactionStatus = TransactionStatus;
module.exports = mongoose.model('BorrowingTransaction', borrowingTransactionSchema);

```
