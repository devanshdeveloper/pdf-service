```javascript
// File: ./features/book-fine/book-fine.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const FineStatus = Object.freeze({
  UNPAID: 'unpaid',
  PARTIALLY_PAID: 'partially_paid',
  OVERDUE: 'overdue',
  PAID: 'paid',
  CANCELLED: 'cancelled'
});
const fineSchema = new Schema({
  finedUser: { type: Schema.Types.ObjectId, ref: 'User', resolveBy: "username", required: true, },
  transaction: { type: Schema.Types.ObjectId, ref: 'BorrowingTransaction', resolveBy: "transactionId", required: true, },
  amount: { type: Number, required: true, min: 0 },
  paidAmount: { type: Number, default: 0, min: 0 },
  status: { type: String, enum: Object.values(FineStatus), default: FineStatus.UNPAID, },
  paidDate: { type: Date },
  user: { type: Schema.Types.ObjectId, ref: 'User', resolveBy: "username", required: true, },
  isDeleted: { type: Boolean, default: false, }
}, { timestamps: true });
module.exports = mongoose.model('BookFine', fineSchema);

```
