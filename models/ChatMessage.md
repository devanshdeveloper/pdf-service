```javascript
// File: ./features/ai/chat-message/chat-message.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const ContentPartSchema = new Schema({
    type: {
        type: String,
        enum: ["text", "tool_call", "image"], // 'tool_result' is usually handled by the message role being 'tool'
        required: true,
    },
    text: String,
    // For tool_call
    toolCallId: String,
    name: String,
    args: Schema.Types.Mixed,
    // For image
    imageUrl: String,
    mimeType: String
}, { _id: false });
const ChatMessageSchema = new Schema(
    {
        session: {
            type: Schema.Types.ObjectId,
            ref: "ChatSession",
            required: true,
        },
        role: {
            type: String,
            enum: ["user", "assistant", "system", "tool"],
            required: true,
        },
        // Support both simple string content (legacy/simple) and structured content parts
        content: {
            type: Schema.Types.Mixed,
            // Can be String or [ContentPartSchema]
            // We'll enforce structure in application logic or pre-save hook
        },
        // For role="tool", we need to link back to the call
        toolCallId: {
            type: String,
        },
        // Metadata for provider info, tokens, etc.
        metadata: {
            provider: String,
            model: String,
            finishReason: String,
            usage: {
                promptTokens: Number,
                completionTokens: Number,
                totalTokens: Number,
            },
            latencyMs: Number,
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
        // Status for tracking streaming/failure
        status: {
            type: String,
            enum: ["pending", "streaming", "completed", "failed", "partial"],
            default: "completed",
        },
        error: {
            message: String,
            stack: String,
        }
        ,
        user: {
            type: Schema.Types.ObjectId,
            ref: "User",
        }
    },
    { timestamps: true }
);
// Indexes
ChatMessageSchema.index({ session: 1, createdAt: 1 }); // For fetching history
ChatMessageSchema.index({ role: 1 });
module.exports = mongoose.model("ChatMessage", ChatMessageSchema);

```
