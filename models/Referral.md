```javascript
// File: ./features/refer-and-earn/referral.model
// models/Referral.js
const mongoose = require('mongoose');
const { Schema } = mongoose;
const ReferralStatus = Object.freeze({
    PENDING: 'pending',
    SUCCESS: 'success',
    EXPIRED: 'expired'
});
const referralSchema = new Schema({
    referrer: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true,
        name: 'Referrer'
    },
    referee: {
        type: Schema.Types.ObjectId, ref: 'User', resolveBy: "username",
        required: true,
        name: 'Referee'
    },
    code: {
        type: String,
        required: true,
        name: 'Used Code',
    },
    status: {
        type: String,
        required: true,
        enum: Object.values(ReferralStatus),
        default: ReferralStatus.PENDING,
        name: 'Status',
    },
    claimedAt: {
        type: Date,
        name: 'Claimed At'
    },
    isDeleted: {
        type: Boolean, default: false,
        name: 'Deleted Flag'
    },
}, {
    timestamps: true
});
module.exports = mongoose.model('Referral', referralSchema);

```
