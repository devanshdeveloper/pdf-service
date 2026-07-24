```javascript
// File: ./features/whatsapp/template/template.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/* ── Button sub-schema ────────────────────────────────────────────────────── */
const buttonSchema = new Schema(
    {
        type: {
            type: String,
            enum: [
                'QUICK_REPLY', 'PHONE_NUMBER', 'URL',
                'OTP', 'CATALOG', 'MPM', 'COPY_CODE',
            ],
        },
        text: String,
        // PHONE_NUMBER
        phoneNumber: String,
        // URL
        url: String,
        urlExample: [String],
        // OTP
        otpType: { type: String, enum: ['COPY_CODE', 'ONE_TAP'] },
        autofillText: String,
        packageName: String,
        signatureHash: String,
    },
    { _id: false }
);

/* ── Component sub-schema ─────────────────────────────────────────────────── */
const componentSchema = new Schema(
    {
        type: {
            type: String,
            enum: ['HEADER', 'BODY', 'FOOTER', 'BUTTONS'],
            required: true,
        },
        // HEADER
        format: {
            type: String,
            enum: ['TEXT', 'IMAGE', 'VIDEO', 'DOCUMENT', 'LOCATION'],
        },
        // TEXT / BODY / FOOTER
        text: String,
        // BODY security
        addSecurityRecommendation: Boolean,
        // FOOTER OTP expiry
        codeExpirationMinutes: Number,
        // Variables example
        example: {
            headerText: [String],
            headerHandle: [String],
            bodyText: [[String]],
        },
        // BUTTONS
        buttons: [buttonSchema],
    },
    { _id: false }
);

/* ── Main schema ─────────────────────────────────────────────────────────── */

/**
 * WhatsApp Message Template
 * API ref: POST /{WABA-ID}/message_templates
 *          GET  /{WABA-ID}/message_templates
 */
const templateSchema = new Schema(
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

        // Meta-assigned template ID
        templateId: { type: String, trim: true, index: true },

        name: {
            type: String,
            required: true,
            trim: true,
            lowercase: true,
            index: true,
        },
        language: { type: String, trim: true, default: 'en_US' },

        category: {
            type: String,
            enum: ['AUTHENTICATION', 'MARKETING', 'UTILITY'],
            required: true,
            index: true,
        },
        previousCategory: { type: String },

        components: [componentSchema],

        // Quality / rejection
        qualityScore: { type: String, trim: true },
        rejectedReason: { type: String, trim: true },

        // Approval lifecycle
        status: {
            type: String,
            enum: ['PENDING', 'APPROVED', 'REJECTED', 'DISABLED', 'IN_APPEAL', 'DELETED'],
            default: 'PENDING',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

templateSchema.index({ user: 1, name: 1, language: 1 });

module.exports = mongoose.model('WhatsAppTemplate', templateSchema);

```
