```javascript
// File: ./features/partner-request/partner-request.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const PartnerRequestSchema = new Schema({
    fullName: { type: String, required: true, trim: true },
    phone: { type: String, required: true, trim: true },
    email: { type: String, lowercase: true, trim: true },
    state: { type: String, trim: true },
    district: { type: String, trim: true },
    profession: {
        type: String,
        enum: ["Teacher", "Tuition / Coaching Owner", "School Staff / Admin", "Education Consultant", "IT / Service Provider", "Freelancer", "Other", ""],
        default: "",
    },
    experience: {
        type: String,
        enum: ["Less than 1 year", "1–3 years", "3–5 years", "5+ years", ""],
        default: "",
    },
    interactsWithSchools: {
        type: String,
        enum: ["yes", "no", ""],
        default: "",
    },
    schoolCount: {
        type: String,
        enum: ["1–5", "6–10", "11–25", "25+", ""],
        default: "",
    },
    partnershipType: {
        type: String,
        enum: ["district-operator", "referral-partner", "not-sure", ""],
        default: "",
    },
    additionalContext: { type: String, trim: true },
    consent: { type: Boolean, default: false },
    status: {
        type: String,
        enum: ["draft", "pending", "approved", "rejected"],
        default: "draft",
    },
    assignedDistrictOperator: {
        type: Schema.Types.ObjectId,
        ref: "DistrictOperator",
        resolveBy: "username",
    },
    isDeleted: { type: Boolean, default: false },
})
module.exports = mongoose.model("PartnerRequest", PartnerRequestSchema);

```
