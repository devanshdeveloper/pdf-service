```javascript
// File: ./features/agency-lead/agency-lead.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");
const agencyLeadSchema = new mongoose.Schema(
    {
        // Contact info
        fullName: {
            type: String,
            trim: true,
            minLength: 2,
            name: "Full Name",
        },
        email: {
            type: String,
            trim: true,
            lowercase: true,
            unique: false, // can have multiple leads with same email
            match: [/^\S+@\S+\.\S+$/, "Invalid email format"],
            name: "Email",
        },
        phone: {
            type: String,
            trim: true,
            name: "Phone Number",
        },
        companyName: { type: String, trim: true, name: "Company" },
        jobTitle: { type: String, trim: true, name: "Job Title" },
        industry: {
            type: String,
            trim: true,
            name: "Industry",
        },
        companySize: {
            type: String,
            name: "Company Size",
        },
        // Lead details
        source: {
            type: String,
            default: "manual",
            name: "Lead Source",
        },
        status: {
            type: String,
            //   enum: ["new", "contacted", "qualified", "proposal_sent", "negotiation", "won", "lost"],
            default: "new",
            name: "Lead Status",
        },
        stage: {
            type: String,
            //   enum: ["awareness", "interest", "consideration", "decision"],
            default: "awareness",
            name: "Pipeline Stage",
        },
        priority: {
            type: String,
            enum: ["low", "medium", "high", "urgent"],
            default: "medium",
            name: "Priority",
        },
        notes: { type: String, trim: true, name: "Notes" },
        // Engagement tracking
        lastContactDate: { type: Date, name: "Last Contact Date" },
        nextFollowUpDate: { type: Date, name: "Next Follow-up" },
        contactedBy: { type: mongoose.Schema.Types.ObjectId, ref: "User", resolveBy: "username", name: "Contacted By" },
        communicationChannel: {
            type: String,
            //   enum: ["email", "phone", "whatsapp", "sms", "meeting", "other"],
            name: "Preferred Communication",
        },
        interactionCount: { type: Number, default: 0, name: "Interaction Count" },
        // Financial info
        estimatedBudget: { type: Number, name: "Estimated Budget" },
        potentialRevenue: { type: Number, name: "Potential Revenue" },
        currency: { type: String, default: "INR", name: "Currency" },
        // Address / Location
        address: {
            type: addressSchema,
        },
        // Lead lifecycle
        createdBy: { type: mongoose.Schema.Types.ObjectId, ref: "User", resolveBy: "username", name: "Created By" },
        assignedTo: { type: mongoose.Schema.Types.ObjectId, ref: "User", resolveBy: "username", name: "Assigned User" },
        user: { type: mongoose.Schema.Types.ObjectId, ref: "User", resolveBy: "username", required: true, name: "Owner" },
        isDeleted: { type: Boolean, default: false },
    },
    { timestamps: true }
);
const AgencyLead = mongoose.model("AgencyLead", agencyLeadSchema);
module.exports = AgencyLead;

```
