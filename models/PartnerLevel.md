```javascript
// File: ./features/partner-level/partner-level.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const partnerLevelSchema = new Schema(
    {
        // ---- Identity ----
        label: {
            type: String,
            required: true,
            trim: true,
            minlength: 2,
            maxlength: 50,
        },
        description: {
            type: String,
            trim: true
        },
        level: {
            type: Number,
            required: true,
            unique: true,
        },
        applicableFor: {
            type: String,
            enum: ["partner", "district_operator"],
            required: true,
        },
        roles: [
            {
                type: String,
                required: true
            }
        ],
        // ---- Commission Rules ----
        commission: {
            type: {
                type: String, // percentage | slab
                enum: ["percentage", "slab"],
                default: "percentage"
            },
            percentage: {
                type: Number,
                default: 0,
                min: 0,
                max: 100
            },
            slabs: [
                {
                    from: {
                        type: Number,
                        required: true,
                        min: 0
                    },
                    to: {
                        type: Number, // null = infinity
                        default: null
                    },
                    percentage: {
                        type: Number,
                        required: true,
                        min: 0,
                        max: 100
                    }
                }
            ],
            calculationBasis: {
                type: String,
                enum: ["monthly", "lifetime"],
                default: "monthly"
            }
        },
        // ---- Upgrade / Qualification Rules ----
        qualificationCriteria: {
            minMonthlyRevenue: {
                type: Number,
                default: 0
            },
            minActiveInstitutions: {
                type: Number,
                default: 0
            },
            minConversions: {
                type: Number,
                default: 0
            }
        },
        // ---- Payout Controls ----
        payout: {
            enabled: {
                type: Boolean,
                default: true
            },
            minimumThreshold: {
                type: Number,
                default: 500
            },
            cycle: {
                type: String,
                enum: ["weekly", "monthly"],
                default: "monthly"
            }
        },
        // ---- Safety & Governance ----
        isDefault: {
            type: Boolean,
            default: false
        },
        isLocked: {
            type: Boolean,
            default: false // once used, prevent edits
        },
        status: {
            type: String,
            enum: ["active", "inactive"],
            default: "active",
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
        // ---- Audit ----
        createdBy: {
            type: Schema.Types.ObjectId,
            ref: "User"
        },
        updatedBy: {
            type: Schema.Types.ObjectId,
            ref: "User"
        }
    },
    {
        timestamps: true
    }
);
const PartnerLevel = mongoose.model("PartnerLevel", partnerLevelSchema);
module.exports = PartnerLevel;

```
