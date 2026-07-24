```javascript
// File: ./features/whatsapp/phone-number/phoneNumber.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp Phone Number
 * Represents a business phone number registered under a WABA.
 * API ref: GET /{WABA-ID}/phone_numbers
 */
const phoneNumberSchema = new Schema(
    {
        user: {
            type: Schema.Types.ObjectId,
            ref: 'User',
            required: true,
            index: true,
        },
        account: {
            type: Schema.Types.ObjectId,
            ref: 'WhatsAppAccount',
            required: true,
            index: true,
        },

        // Meta identifiers
        phoneNumberId: {
            type: String,
            required: true,
            trim: true,
            comment: 'Graph API phone number ID',
        },
        displayPhoneNumber: { type: String, trim: true },
        verifiedName: { type: String, trim: true },

        // Quality
        qualityRating: {
            type: String,
            enum: ['GREEN', 'YELLOW', 'RED', 'NA'],
            default: 'NA',
        },

        // Display name approval
        nameStatus: {
            type: String,
            enum: [
                'APPROVED',
                'AVAILABLE_WITHOUT_REVIEW',
                'DECLINED',
                'EXPIRED',
                'PENDING_REVIEW',
                'NONE',
            ],
            default: 'NONE',
        },

        // Registration state
        isRegistered: { type: Boolean, default: false },
        isOfficialBusinessAccount: { type: Boolean, default: false },
        accountMode: {
            type: String,
            enum: ['LIVE', 'SANDBOX'],
            default: 'LIVE',
        },

        // Two-step verification (pin stored hashed or not stored)
        twoStepVerificationEnabled: { type: Boolean, default: false },

        // Verification workflow
        verificationMethod: { type: String, enum: ['SMS', 'VOICE'] },
        verificationLocale: { type: String, trim: true },

        status: {
            type: String,
            enum: ['active', 'inactive', 'flagged', 'restricted', 'unregistered'],
            default: 'unregistered',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

phoneNumberSchema.index({ user: 1, phoneNumberId: 1 }, { unique: true });

module.exports = mongoose.model('WhatsAppPhoneNumber', phoneNumberSchema);

```
