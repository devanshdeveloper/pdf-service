```javascript
// File: ./features/whatsapp/qr-code/qrCode.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp QR Code
 * Manages QR codes that link to a WhatsApp chat.
 * API ref: references/qr-codes.md
 *          GET/POST/DELETE /{Phone-Number-ID}/message_qrdls
 */
const qrCodeSchema = new Schema(
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
            required: true,
        },

        // Meta-assigned QR code ID
        qrCodeId: {
            type: String,
            trim: true,
            index: true,
            comment: 'ID returned by /{Phone-Number-ID}/message_qrdls',
        },

        // Pre-filled message shown when the QR is scanned
        prefillMessage: { type: String, trim: true },

        // The deep link / URL encoded in the QR
        deepLinkUrl: { type: String, trim: true },

        // The raw QR image (base64 or hosted URL)
        qrImageUrl: { type: String, trim: true },

        // Scan analytics (lightweight - use WhatsAppAnalytics for full metrics)
        totalScans: { type: Number, default: 0 },

        // Lifecycle
        status: {
            type: String,
            enum: ['active', 'inactive', 'deleted'],
            default: 'active',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

qrCodeSchema.index({ user: 1, qrCodeId: 1 }, { sparse: true });

module.exports = mongoose.model('WhatsAppQrCode', qrCodeSchema);

```
