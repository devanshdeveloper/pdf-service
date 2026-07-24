```javascript
// File: ./features/fee (voucher-based)/fee/fee.model
const { FeeStatusTypes, FeeStatusTypesArray } = require('./fee');
const mongoose = require('mongoose');
const feeSchema = new mongoose.Schema({
  student: {
    type: mongoose.Schema.Types.ObjectId,
    ref: "Student",
    required: true
  },
  name: {
    type: String,
    required: true
  },
  currency: {
    type: String,
    required: true
  },
  type: {
    type: String,
    required: true,
    enum: ["installments", "one-time"]
  },
  installments: [
    {
      name: { type: String, required: true },
      discount: { type: Number, min: 0 },
      fineable: { type: Boolean, default: false },
      fineFrequency: { type: String, enum: ['daily', 'weekly', 'monthly'] },
      fineAmount: { type: Number, min: 0 },
      status: { type: String, enum: FeeStatusTypesArray, default: FeeStatusTypes.Draft },
      dueDate: { type: Date, required: true },
      installmentTotal: { type: Number, min: 0 },
      paidAmount: { type: Number, min: 0 },
      dueAmount: { type: Number, min: 0 },
      history: [
        { type: mongoose.Schema.Types.ObjectId, ref: "Ledger" }
      ],
      items: [
        {
          name: { type: String, required: true },
          amount: { type: Number, required: true, min: 0 },
          discount: { type: Number, min: 0 },
          lineTotal: { type: Number, min: 0 },
          isEnabled: { type: Boolean, }
        }
      ],
    }
  ],
  status: { type: String, enum: FeeStatusTypesArray, default: FeeStatusTypes.Draft },
  totalAmount: { type: Number, min: 0 },
  paidAmount: { type: Number, min: 0 },
  dueAmount: { type: Number, min: 0 },
  isDeleted: { type: Boolean, default: false, },
  user: {
    type: mongoose.Schema.Types.ObjectId,
    ref: "User", resolveBy: "username",
    required: true
  }
}, { timestamps: true });
const Fee = mongoose.model('Fee', feeSchema);
module.exports = Fee;
```
