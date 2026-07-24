```javascript
// File: ./features/refer-and-earn/reward.model
// models/Reward.js
const mongoose = require('mongoose');
const { Schema } = mongoose;
const RewardType = Object.freeze({
    CASH: 'cash',
    DISCOUNT: 'discount',
    CREDIT: 'credit'
});
const RewardStatus = Object.freeze({
    UNCLAIMED: 'unclaimed',
    CLAIMED: 'claimed',
    EXPIRED: 'expired'
});
const rewardSchema = new Schema({
    user: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true,
        name: 'Rewarded User',
    },
    type: {
        type: String,
        required: true,
        enum: Object.values(RewardType),
        name: 'Reward Type'
    },
    amount: {
        type: Number,
        required: true, min: [0, 'Amount must be non-negative'],
        name: 'Amount'
    },
    status: {
        type: String,
        required: true,
        enum: Object.values(RewardStatus),
        default: RewardStatus.UNCLAIMED,
        name: 'Status'
    },
    issuedAt: {
        type: Date, default: Date.now,
        name: 'Issued At'
    },
    claimedAt: {
        type: Date,
        name: 'Claimed At'
    },
    expiresAt: {
        type: Date,
        name: 'Expires At'
    },
    isDeleted: {
        type: Boolean, default: false,
        name: 'Deleted Flag'
    },
}, {
    timestamps: true
});
module.exports = mongoose.model('reward', rewardSchema);

```
