```javascript
// File: ./features/ai/ai-agent/ai-agent.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const AIAgentSchema = new Schema(
    {
        name: {
            type: String,
            required: true,
        },
        description: {
            type: String,
        },
        // The core personality/instruction
        basePrompt: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "BasePrompt",
            required: true,
        },
        // Tools this agent is allowed to use
        allowedTools: [{
            type: String,
        }],
        createdBy: {
            type: Schema.Types.ObjectId,
            ref: "User",
        },
        avatar: String,
        uiOptions: {
            welcomeMessage: String,
            suggestedPrompts: [String],
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
        status: {
            type: String,
            enum: ["active", "inactive"],
            default: "active",
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User",
        }
    },
    { timestamps: true }
);
module.exports = mongoose.model("AIAgent", AIAgentSchema);

```
