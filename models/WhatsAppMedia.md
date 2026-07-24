```javascript
// File: ./features/whatsapp/media/media.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp Media
 * Tracks uploaded media objects (images, audio, video, documents, stickers).
 * API ref: POST /{Phone-Number-ID}/media
 *          GET  /{Media-ID}
 *          DELETE /{Media-ID}
 */
const mediaSchema = new Schema(
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

        // Meta-assigned media ID returned after upload
        mediaId: {
            type: String,
            required: true,
            trim: true,
            index: true,
            comment: 'ID returned by the WhatsApp media upload endpoint',
        },

        // Media classification
        mediaType: {
            type: String,
            enum: ['image', 'audio', 'video', 'document', 'sticker'],
            required: true,
            index: true,
        },

        // Mime type (e.g. image/jpeg, audio/ogg, video/mp4)
        mimeType: { type: String, trim: true },

        // Integrity
        sha256: { type: String, trim: true },

        // File metadata
        fileSize: { type: Number, comment: 'bytes' },
        filename: { type: String, trim: true },

        // Temporary download URL (valid 5 minutes after retrieval)
        downloadUrl: { type: String, trim: true },
        downloadUrlExpiresAt: { type: Date },

        // Local / CDN path if we re-host the file
        localPath: { type: String, trim: true },
        localUrl: { type: String, trim: true },

        // Lifecycle
        status: {
            type: String,
            enum: ['uploaded', 'retrieved', 'expired', 'deleted'],
            default: 'uploaded',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

mediaSchema.index({ user: 1, mediaId: 1 }, { unique: true });

module.exports = mongoose.model('WhatsAppMedia', mediaSchema);

```
