```javascript
// File: ./features/whatsapp/message/message.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/* ── Sub-schemas ─────────────────────────────────────────────────────────── */

const mediaObjectSchema = new Schema(
    {
        id: { type: String, trim: true },           // WhatsApp media id
        link: { type: String, trim: true },         // public URL alternative
        caption: { type: String, trim: true },
        filename: { type: String, trim: true },
        mimeType: { type: String, trim: true },
        sha256: { type: String, trim: true },
        voice: { type: Boolean },                   // audio-as-voice-note flag
    },
    { _id: false }
);

const locationObjectSchema = new Schema(
    {
        latitude: { type: Number },
        longitude: { type: Number },
        name: { type: String, trim: true },
        address: { type: String, trim: true },
    },
    { _id: false }
);

const contactAddressSchema = new Schema(
    {
        street: String, city: String, state: String,
        zip: String, country: String, countryCode: String,
        type: { type: String, enum: ['HOME', 'WORK'] },
    },
    { _id: false }
);

const contactSchema = new Schema(
    {
        name: {
            formattedName: String,
            firstName: String, lastName: String,
            middleName: String, suffix: String, prefix: String,
        },
        birthday: String,
        phones: [{ phone: String, waId: String, type: String }],
        emails: [{ email: String, type: String }],
        urls: [{ url: String, type: String }],
        addresses: [contactAddressSchema],
        org: { company: String, department: String, title: String },
    },
    { _id: false }
);

const interactiveSchema = new Schema(
    {
        type: {
            type: String,
            enum: ['button', 'list', 'product', 'product_list', 'flow'],
        },
        header: Schema.Types.Mixed,
        body: { text: String },
        footer: { text: String },
        action: Schema.Types.Mixed,
    },
    { _id: false }
);

const reactionSchema = new Schema(
    {
        messageId: { type: String, trim: true },
        emoji: { type: String, trim: true },
    },
    { _id: false }
);

const templateMessageSchema = new Schema(
    {
        name: { type: String, trim: true },
        language: { code: String },
        components: { type: Schema.Types.Mixed },
    },
    { _id: false }
);

const referralSchema = new Schema(
    {
        sourceUrl: String,
        sourceId: String,
        sourceType: { type: String, enum: ['ad', 'post'] },
        headline: String,
        body: String,
        mediaType: { type: String, enum: ['image', 'video'] },
        imageUrl: String,
        videoUrl: String,
        thumbnailUrl: String,
    },
    { _id: false }
);

/* ── Main schema ─────────────────────────────────────────────────────────── */

/**
 * WhatsApp Message
 * Stores both outbound (sent) and inbound (received via webhook) messages.
 * API ref: POST /{Phone-Number-ID}/messages
 * Webhook ref: webhook-subscriptions.md
 */
const messageSchema = new Schema(
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
        phoneNumber: {
            type: Schema.Types.ObjectId,
            ref: 'WhatsAppPhoneNumber',
        },

        // Direction
        direction: {
            type: String,
            enum: ['outbound', 'inbound'],
            required: true,
            index: true,
        },

        // WhatsApp assigned IDs
        wamid: {
            type: String,
            trim: true,
            index: true,
            comment: 'WhatsApp message ID (wamid.xxx)',
        },

        // Addressing
        to: { type: String, trim: true },
        from: { type: String, trim: true },     // populated for inbound
        waId: { type: String, trim: true },     // sender wa_id from webhook

        messagingProduct: { type: String, default: 'whatsapp' },
        recipientType: { type: String, default: 'individual' },

        // Message type — mirrors WhatsApp API type field
        type: {
            type: String,
            enum: [
                'text', 'image', 'audio', 'video', 'document', 'sticker',
                'location', 'contacts', 'interactive', 'template',
                'reaction', 'order', 'unknown',
            ],
            required: true,
            index: true,
        },

        // Type-specific payloads — only the relevant field is set
        text: {
            body: String,
            previewUrl: Boolean,
        },
        image: { type: mediaObjectSchema },
        audio: { type: mediaObjectSchema },
        video: { type: mediaObjectSchema },
        document: { type: mediaObjectSchema },
        sticker: { type: mediaObjectSchema },
        location: { type: locationObjectSchema },
        contacts: [contactSchema],
        interactive: { type: interactiveSchema },
        template: { type: templateMessageSchema },
        reaction: { type: reactionSchema },

        // Reply context (when replying to a previous message)
        context: {
            messageId: { type: String, trim: true },
            from: { type: String, trim: true },
        },

        // Click-to-WhatsApp ads referral
        referral: { type: referralSchema },

        // Delivery tracking (outbound)
        deliveryStatus: {
            type: String,
            enum: ['queued', 'sent', 'delivered', 'read', 'failed', 'deleted'],
            default: 'queued',
            index: true,
        },
        sentAt: { type: Date },
        deliveredAt: { type: Date },
        readAt: { type: Date },
        failedAt: { type: Date },
        failReason: { type: String },

        // Inbound timestamp from webhook
        whatsappTimestamp: { type: Number },

        // Errors from webhook unknown-message payload
        errors: [
            {
                code: Number,
                title: String,
                details: String,
            },
        ],

        // Lifecycle
        status: {
            type: String,
            enum: ['pending', 'sent', 'delivered', 'failed', 'received'],
            default: 'pending',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

messageSchema.index({ user: 1, direction: 1, createdAt: -1 });
messageSchema.index({ wamid: 1 }, { sparse: true });

module.exports = mongoose.model('WhatsAppMessage', messageSchema);

```
