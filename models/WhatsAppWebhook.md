```javascript
// File: ./features/whatsapp/webhook/webhook.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp Webhook Event Log
 * Persists every raw webhook payload received from Meta for auditing,
 * replay, and debugging. A separate message/status record is created
 * from the parsed data; this model stores the raw envelope.
 * API ref: webhook-subscriptions.md — Webhook Payload Reference
 */
const webhookSchema = new Schema(
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
            index: true,
        },

        // Meta envelope
        object: { type: String, default: 'whatsapp_business_account' },
        wabaId: { type: String, trim: true, index: true },

        // Parsed change field (e.g. "messages")
        field: { type: String, trim: true, index: true },

        // Event classification
        eventType: {
            type: String,
            enum: [
                'message_received',
                'message_sent',
                'message_delivered',
                'message_read',
                'message_failed',
                'message_deleted',
                'template_status_update',
                'account_update',
                'business_capability_update',
                'unknown',
            ],
            default: 'unknown',
            index: true,
        },

        // Phone metadata from value.metadata
        displayPhoneNumber: { type: String, trim: true },
        phoneNumberId: { type: String, trim: true },

        // Raw payload stored for replay / audit
        rawPayload: { type: Schema.Types.Mixed },

        // Link to processed message (if applicable)
        relatedMessage: {
            type: Schema.Types.ObjectId,
            ref: 'WhatsAppMessage',
        },

        // Processing state
        processed: { type: Boolean, default: false, index: true },
        processingError: { type: String },

        // Lifecycle
        status: {
            type: String,
            enum: ['received', 'processed', 'failed', 'ignored'],
            default: 'received',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

webhookSchema.index({ user: 1, eventType: 1, createdAt: -1 });
webhookSchema.index({ wabaId: 1, createdAt: -1 });

module.exports = mongoose.model('WhatsAppWebhook', webhookSchema);

```
