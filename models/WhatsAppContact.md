```javascript
// File: ./features/whatsapp/contact/contact.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp Contact
 * Stores WhatsApp contacts received via inbound contact-type messages.
 * Normalised out of the message payload for reuse across conversations.
 * API ref: webhook-subscriptions.md — Received Contact Messages
 */
const contactSchema = new Schema(
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
        },

        // WhatsApp identifier
        waId: { type: String, trim: true, index: true },

        // Name
        name: {
            formattedName: { type: String, trim: true },
            firstName: { type: String, trim: true },
            lastName: { type: String, trim: true },
            middleName: { type: String, trim: true },
            suffix: { type: String, trim: true },
            prefix: { type: String, trim: true },
        },

        birthday: { type: String, trim: true },

        phones: [
            {
                phone: { type: String, trim: true },
                waId: { type: String, trim: true },
                type: { type: String, enum: ['HOME', 'WORK', 'MOBILE', 'MAIN', 'IPHONE', 'OTHER'] },
            },
        ],

        emails: [
            {
                email: { type: String, trim: true, lowercase: true },
                type: { type: String, enum: ['HOME', 'WORK', 'OTHER'] },
            },
        ],

        urls: [
            {
                url: { type: String, trim: true },
                type: { type: String, enum: ['HOME', 'WORK', 'OTHER'] },
            },
        ],

        addresses: [
            {
                street: String,
                city: String,
                state: String,
                zip: String,
                country: String,
                countryCode: String,
                type: { type: String, enum: ['HOME', 'WORK', 'OTHER'] },
            },
        ],

        org: {
            company: String,
            department: String,
            title: String,
        },

        // Conversation tracking
        lastMessageAt: { type: Date, index: true },
        messageCount: { type: Number, default: 0 },

        // Opt-out management
        optedOut: { type: Boolean, default: false, index: true },
        optedOutAt: { type: Date },

        status: {
            type: String,
            enum: ['active', 'blocked', 'opted_out'],
            default: 'active',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

contactSchema.index({ user: 1, waId: 1 }, { unique: true, sparse: true });

module.exports = mongoose.model('WhatsAppContact', contactSchema);

```
