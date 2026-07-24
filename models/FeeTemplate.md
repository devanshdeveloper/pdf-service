```javascript
// File: ./features/fee (voucher-based)/fee-template/fee-template.model
const mongoose = require('mongoose');
const feeTemplateSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    currency: {
        type: String,
        required: true
    },
    month: {
        type: String,
        required: true,
        enum: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
    },
    hasInstallments: { type: Boolean, default: false },
    installments: [
        {
            name: { type: String, required: true },
            items: [
                {
                    name: { type: String, required: true },
                    amount: { type: Number, required: true, min: 0 },
                    frequency: { type: String, enum: ['one-time', 'monthly', "yearly"], required: true },
                }
            ],
        }
    ],
    fineable: { type: Boolean, default: false },
    fineAmount: { type: Number, min: 0 },
    fineFrequency: { type: String, enum: ['daily', 'weekly', 'monthly'] },
    status: { type: String, enum: ['active', 'inactive'], default: 'active', },
    isDeleted: { type: Boolean, default: false, },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true
    }
}, { timestamps: true });
const FeeTemplate = mongoose.model('FeeTemplate', feeTemplateSchema);
module.exports = FeeTemplate;
```
