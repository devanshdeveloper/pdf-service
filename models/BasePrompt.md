```javascript
// File: ./features/ai/base-prompt/base-prompt.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const BasePromptSchema = new Schema(
    {
        slug: {
            type: String,
            required: true,
            unique: true, // This enforces uniqueness on the slug, so versioning needs to be handled carefully. 
            // Actually, we might want unique composite index on { slug: 1, version: 1 }.
            // But typically "slug" refers to the *family* of prompts.
            // Let's make slug + version unique.
        },
        version: {
            type: Number,
            required: true,
            default: 1,
        },
        template: {
            type: String,
            required: true,
            // Example: "You are a helpful assistant for {{subject}}..."
        },
        inputVariables: [{
            type: String,
        }],
        description: String,
        isActive: {
            type: Boolean,
            default: true,
        },
        // Allowed roles (who can use this prompt)
        allowedRoles: [{
            type: String,
            // e.g., 'teacher', 'student', 'admin'
        }],
        // Metadata for provider-specific configs if needed
        config: {
            temperature: Number,
            topP: Number,
            model: String, // Preferred model
        },
        createdBy: {
            type: Schema.Types.ObjectId,
            ref: "User",
        },
        status: {
            type: String,
            default: "active",
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
        user: {
            type: Schema.Types.ObjectId,
            ref: "User",
        }
    },
    { timestamps: true }
);
module.exports = mongoose.model("BasePrompt", BasePromptSchema);

```
