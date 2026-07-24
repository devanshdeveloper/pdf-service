```javascript
// File: ./features/ai/chat-session/chat-session.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const ChatSessionSchema = new Schema(
    {
        user: {
            type: Schema.Types.ObjectId,
            ref: "User",
            required: true,
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
        title: {
            type: String,
            default: "New Conversation",
        },
        mode: {
            type: String,
            enum: ["chat", "agent", "exam_generator"],
            default: "chat",
        },
        agent: {
            type: Schema.Types.ObjectId,
            ref: "AIAgent",
        },
        status: {
            type: String,
            enum: ["active", "archived", "deleted"],
            default: "active",
        },
        stats: {
            messageCount: { type: Number, default: 0 },
            tokenUsage: { type: Number, default: 0 },
        },
        lastMessageAt: {
            type: Date,
            default: Date.now,
        },
        metadata: {
            type: Map,
            of: String,
        },
    },
    { timestamps: true }
);
// Indexes for common queries
ChatSessionSchema.index({ user: 1, createdAt: -1 });
ChatSessionSchema.index({ institute: 1 });
ChatSessionSchema.index({ status: 1 });
module.exports = mongoose.model("ChatSession", ChatSessionSchema);

```
