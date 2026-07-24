```javascript
// File: ./features/whatsapp/business-profile/businessProfile.model
const mongoose = require('mongoose');

const { Schema } = mongoose;

/**
 * WhatsApp Business Profile
 * Stores the public-facing business profile fields for a phone number.
 * API ref: references/business-profiles.md
 *          GET/POST /{Phone-Number-ID}/whatsapp_business_profile
 */
const businessProfileSchema = new Schema(
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
            unique: true,
        },

        // Profile fields (mirrors Graph API response)
        about: { type: String, trim: true, maxlength: 139 },
        description: { type: String, trim: true, maxlength: 512 },
        address: { type: String, trim: true, maxlength: 256 },
        email: { type: String, trim: true, lowercase: true },
        websites: [{ type: String, trim: true }],

        vertical: {
            type: String,
            trim: true,
            comment:
                'Business category e.g. RETAIL, EDUCATION, FINANCE, HEALTH etc.',
        },

        // Profile photo (WhatsApp media handle or local URL)
        profilePictureUrl: { type: String, trim: true },
        profilePictureHandle: { type: String, trim: true },

        // Messaging hours / awa settings (extended)
        messagingHoursEnabled: { type: Boolean, default: false },
        messagingHours: [
            {
                dayOfWeek: {
                    type: String,
                    enum: ['MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN'],
                },
                openTime: String,   // e.g. "09:00"
                closeTime: String,  // e.g. "18:00"
            },
        ],

        // Lifecycle
        status: {
            type: String,
            enum: ['active', 'inactive', 'pending_review'],
            default: 'active',
            index: true,
        },
        isDeleted: { type: Boolean, default: false, index: true },
    },
    { timestamps: true }
);

module.exports = mongoose.model('WhatsAppBusinessProfile', businessProfileSchema);

```
