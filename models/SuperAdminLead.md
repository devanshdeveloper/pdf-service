```javascript
// File: ./features/super-admin-lead/super-admin-lead.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");
const { Schema } = mongoose;
// Document sub-schema
const documentSchema = new Schema(
    {
        label: {
            type: String,
            name: "Document Label",
        },
        value: {
            type: String,
            name: "Document URL",
        },
        identifier: {
            type: String,
            name: "Document Identifier"
        }
    },
    { _id: false }
);
const SuperAdminLeadSchema = new Schema(
    {
        /* =========================
           CORE CONTACT INFORMATION
        ========================== */
        avatar: {
            type: String,
            name: "Avatar URL"
        },
        firstName: {
            type: String,
            trim: true,
            index: true
        },
        lastName: {
            type: String,
            trim: true,
            index: true
        },
        email: {
            type: String,
            lowercase: true,
            trim: true,
            index: true
        },
        phone: {
            type: String,
            trim: true,
            index: true
        },
        contactRole: {
            type: String,
            trim: true
        },
        /* =========================
           INSTITUTE CONTEXT
        ========================== */
        instituteName: {
            type: String,
            trim: true
        },
        instituteTypes: {
            type: [String]
        },
        instituteBoards: {
            type: [String]
        },
        instituteMediums: {
            type: [String]
        },
        noOfStudents: {
            type: Number
        },
        preferredDemoTime: {
            type: String,
            enum: [
                "Morning (9 AM - 12 PM)",
                "Afternoon (12 PM - 3 PM)",
                "Evening (3 PM - 6 PM)"
            ]
        },
        /* =========================
           BUSINESS CONTEXT
        ========================== */
        businessName: {
            type: String,
            trim: true
        },
        industry: {
            type: String,
            enum: [
                "education",
                "ecommerce",
                "manufacturing",
                "finance",
                "salon",
                "gym",
                "agency",
                "service",
                "other"
            ],
            index: true
        },
        organizationSize: {
            type: String,
            enum: ["1-10", "11-50", "51-200", "200+"]
        },
        address: {
            type: addressSchema,
            trim: true
        },
        /* =========================
           PRODUCT & INTENT
        ========================== */
        interestedProducts: [
            {
                type: String,
                enum: [
                    "NextWeb Campus",
                    "NextWeb Commerce",
                    "NextWeb Inventory",
                    "NextWeb Finance",
                    "NextWeb People",
                    "NextWeb Plant",
                    "NextWeb FitHub",
                    "NextWeb SalonHub",
                    "NextWeb Agency",
                    "NextWeb Procure",
                    "NextWeb FieldOps",
                    "NextWeb Projects",
                    "NextWeb Analytics",
                    "NextWeb HelpDesk",
                    "NextWeb Compliance"
                ]
            }
        ],
        message: {
            type: String,
            trim: true
        },
        /* =========================
           LEAD SOURCE & ATTRIBUTION
        ========================== */
        source: {
            type: String,
            enum: [
                "website",
                "product_page",
                "facebook_ads",
                "google_ads",
                "linkedin",
                "whatsapp",
                "referral",
                "partner",
                "manual"
            ],
            required: true,
            index: true
        },
        landingPage: {
            type: String
        },
        utm: {
            source: String,
            medium: String,
            campaign: String,
            content: String,
            term: String
        },
        adMeta: {
            platform: {
                type: String,
                enum: ["facebook", "google", "linkedin"]
            },
            campaignId: String,
            adSetId: String,
            adId: String
        },
        fields: {
            type: [
                {
                    label: {
                        type: String,
                        required: true,
                    },
                    value: {
                        type: Schema.Types.Mixed,
                        required: true,
                    },
                },
            ],
            name: "Custom Fields",
        },
        /* =========================
           SALES PIPELINE (SUPER ADMIN)
        ========================== */
        status: {
            type: String,
            enum: [
                "new",
                "contacted",
                "qualified",
                "demo_scheduled",
                "proposal_sent",
                "converted",
                "lost",
                "junk"
            ],
            default: "new",
            index: true
        },
        assignedTo: {
            type: Schema.Types.ObjectId,
            ref: "AdminUser" // sales / super admin user
        },
        nextFollowUpAt: {
            type: Date,
            index: true
        },
        /* =========================
           SYSTEM & QUALITY CONTROL
        ========================== */
        isDuplicate: {
            type: Boolean,
            default: false
        },
        duplicateOf: {
            type: Schema.Types.ObjectId,
            ref: "SuperAdminLeads"
        },
        ipAddress: {
            type: String
        },
        userAgent: {
            type: String
        },
        isSpam: {
            type: Boolean,
            default: false
        },
        isDeleted: {
            type: Boolean,
            default: false
        },
        /* =========================
           AUDIT
        ========================== */
        createdBy: {
            type: String,
            enum: ["system", "admin"],
            default: "system"
        },
        documents: {
            type: [documentSchema],
            default: [],
            name: "Documents",
        },
        identifiers: {
            type: [
                {
                    label: {
                        type: String,
                        trim: true,
                        minlength: [2, "Identifier label must be at least 2 characters"],
                        maxlength: [50, "Identifier label cannot exceed 50 characters"],
                    },
                    value: {
                        type: String,
                        trim: true,
                        maxlength: [100, "Identifier value cannot exceed 100 characters"],
                    },
                },
            ],
            name: "Identifiers",
        },
        username: {
            type: String,
            unique: true,
            trim: true,
            sparse: true,
            name: "Username",
        },
        password: {
            type: String,
            name: "Password",
        },
        userId: {
            type: Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            name: "User ID",
        },
        notes: {
            type: String,
            trim: true,
        },
        comments: [{
            message: {
                type: String,
                trim: true,
            },
            author: {
                type: mongoose.Schema.Types.ObjectId,
                ref: "User", resolveBy: "username",
            },
            createdAt: {
                type: Date,
                default: Date.now,
            },
        }],
    },
    {
        timestamps: true,
        versionKey: false
    }
);
module.exports = mongoose.model("SuperAdminLead", SuperAdminLeadSchema);

```
