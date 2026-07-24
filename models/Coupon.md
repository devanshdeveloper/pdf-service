```javascript
// File: ./features/coupon-code/coupon-code.model
// models/Coupon.js
const mongoose = require('mongoose');
const couponSchema = new mongoose.Schema({
    // Basic Info
    code: {
        type: String,
        required: true,
        unique: true,
        uppercase: true,
        trim: true
    },
    description: {
        type: String,
        trim: true
    },
    status: {
        type: String,
        enum: ['active', 'paused', 'expired'],
        default: 'active'
    },
    // Discount Configuration
    discountType: {
        type: String,
        enum: ['percentage', 'fixed', 'buyXgetY', 'fixedPrice'],
        required: true
    },
    discountValue: {
        type: Number,
        required: true,
        min: 0
    },
    maxDiscountAmount: {
        type: Number,
        min: 0 // For percentage discounts
    },
    // Usage Limits
    maxUses: {
        type: Number,
        default: 1,
        min: 1
    },
    usedCount: {
        type: Number,
        default: 0,
        min: 0
    },
    maxUsesPerUser: {
        type: Number,
        default: 1,
        min: 1
    },
    // Validity Period
    startsAt: {
        type: Date,
        default: Date.now
    },
    expiresAt: {
        type: Date,
        required: true
    },
    // Usage Requirements
    minimumPurchaseAmount: {
        type: Number,
        min: 0,
        default: 0
    },
    // Applicability
    applicablePlans: [{
        type: mongoose.Schema.Types.ObjectId,
        ref: 'Plan'
    }],
    isDeleted: {
        type: Boolean,
        default: false
    }
}, {
    timestamps: true,
    toJSON: { virtuals: true },
    toObject: { virtuals: true }
});
module.exports = mongoose.model('Coupon', couponSchema);

```
