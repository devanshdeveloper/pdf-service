```javascript
// File: ./features/refer-and-earn/referral-code.model
const mongoose = require('mongoose');
const { Schema } = mongoose;
const referralCodeSchema = new Schema({
    code: {
        type: String,
        required: true,
        unique: true,
        name: 'Referral Code',
    },
    user: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        required: true,
        name: 'User',
    },
    status: {
        type: String,
        enum: ['active', 'inactive'],
        default: 'active',
        name: 'Status'
    },
    isDeleted: {
        type: Boolean,
        default: false,
        name: 'Deleted Flag'
    },
}, {
    timestamps: true
});
module.exports = mongoose.model('referralcode', referralCodeSchema);

```
