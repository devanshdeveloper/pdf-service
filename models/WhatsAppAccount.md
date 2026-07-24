```javascript
// File: ./features/whatsapp/account/account.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp Business Account (WABA)
 * Represents a WhatsApp Business Account registered with the Cloud API.
 * One per tenant; referenced by all other WhatsApp models.
 */
const accountSchema = new Schema(
    {
        user: {
            type: Schema.Types.ObjectId,
            ref: 'User',
            required: true,
            index: true,
        },

        // Meta / Graph API identifiers
        wabaId: {
            type: String,
            required: true,
            trim: true,
            comment: 'WhatsApp Business Account ID from Meta',
        },
        business: {
            type: String,
            trim: true,
            comment: 'Meta Business Portfolio ID',
        },
        name: { type: String, trim: true },
        timezoneId: { type: String, trim: true },
        messageTemplateNamespace: { type: String, trim: true },
        currency: { type: String, trim: true, default: 'USD' },

        // Access token management
        accessToken: { type: String, select: false },
        tokenExpiresAt: { type: Date },
        tokenType: {
            type: String,
            enum: ['user', 'system_user'],
            default: 'system_user',
        },

        // Webhook subscription
        webhookSubscribed: { type: Boolean, default: false },
        webhookCallbackUrl: { type: String, trim: true },
        webhookVerifyToken: { type: String, select: false },

        // API version in use (e.g. "v17.0")
        apiVersion: { type: String, default: 'v17.0' },

        // Lifecycle
        status: {
            type: String,
            enum: ['active', 'inactive', 'suspended', 'pending'],
            default: 'pending',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

accountSchema.index({ user: 1, wabaId: 1 }, { unique: true });

module.exports = mongoose.model('WhatsAppAccount', accountSchema);

```
