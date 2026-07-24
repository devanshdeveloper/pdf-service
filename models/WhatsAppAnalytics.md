```javascript
// File: ./features/whatsapp/analytics/analytics.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp Analytics Snapshot
 * Stores a daily/weekly analytics snapshot per phone number.
 * API ref: references/analytics.md
 *          GET /{WABA-ID}?fields=analytics
 * Granularity: DAY | HALF_HOUR | MONTH
 */
const analyticsSchema = new Schema(
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
        phoneNumber: {
            type: Schema.Types.ObjectId,
            ref: 'WhatsAppPhoneNumber',
            index: true,
        },

        // Period
        granularity: {
            type: String,
            enum: ['DAY', 'HALF_HOUR', 'MONTH'],
            required: true,
        },
        periodStart: { type: Date, required: true, index: true },
        periodEnd: { type: Date },

        // Message volume
        sent: { type: Number, default: 0 },
        delivered: { type: Number, default: 0 },
        read: { type: Number, default: 0 },
        received: { type: Number, default: 0 },

        // Conversation-based pricing buckets (Meta billing model)
        conversations: {
            businessInitiated: { type: Number, default: 0 },
            userInitiated: { type: Number, default: 0 },
            // breakdown by category
            marketing: { type: Number, default: 0 },
            utility: { type: Number, default: 0 },
            authentication: { type: Number, default: 0 },
            service: { type: Number, default: 0 },
            free: { type: Number, default: 0 },
        },

        // Cost (if billing data returned)
        cost: {
            amount: { type: Number },
            currency: { type: String, trim: true },
        },

        // Quality info snapshot
        qualityRating: {
            type: String,
            enum: ['GREEN', 'YELLOW', 'RED', 'NA'],
        },

        // Lifecycle
        status: {
            type: String,
            enum: ['raw', 'aggregated', 'archived'],
            default: 'raw',
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

analyticsSchema.index({ user: 1, account: 1, granularity: 1, periodStart: 1 });

module.exports = mongoose.model('WhatsAppAnalytics', analyticsSchema);

```
