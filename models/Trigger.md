```javascript
// File: ./features/trigger/trigger.model
const mongoose = require("mongoose");
const TriggerSchema = new mongoose.Schema(
    {
        // Human readable
        name: {
            type: String,
            required: true,
            trim: true,
        },
        description: {
            type: String,
            trim: true,
        },
        // ENABLE / DISABLE
        isActive: {
            type: Boolean,
            default: true,
            index: true,
        },
        /**
         * triggerType defines HOW it fires
         * - schedule  -> cron / delayed
         * - event     -> entity based
         */
        triggerType: {
            type: String,
            enum: ["schedule", "event"],
            required: true,
            index: true,
        },
        /**
         * SCHEDULE CONFIG
         * Used only when triggerType = schedule
         */
        schedule: {
            // Cron expression for recurring triggers
            cron: [{
                type: String,
                trim: true,
            }],
            // One-time execution date
            runAt: {
                type: Date,
            },
            // Timezone for cron execution
            timezone: {
                type: String,
                default: "Asia/Kolkata",
            },
        },
        /**
         * EVENT CONFIG
         * Used only when triggerType = event
         */
        event: {
            // Entity name e.g. "product", "student", "invoice"
            entity: {
                type: String,
                trim: true,
                lowercase: true,
            },
            // Action type: create | update | delete | status_change
            action: {
                type: String,
                enum: ["create", "update", "delete", "status_change"],
            },
            // Match conditions for the entity data
            match: {
                type: mongoose.Schema.Types.Mixed,
            },
            /*
              Example match:
              {
                status: "pending",
                amount: { $gt: 0 }
              }
            */
        },
        /**
         * CONDITIONS (Optional)
         * Additional runtime filters applied before action execution
         */
        conditions: {
            type: mongoose.Schema.Types.Mixed,
        },
        /**
         * ACTIONS TO EXECUTE
         * Array of actions to perform when trigger fires
         */
        actions: [
            {
                type: {
                    type: String,
                    enum: ["email", "webhook", "job", "db", "export"],
                    required: true,
                },
                id: {
                    type: String,
                    default: () => new mongoose.Types.ObjectId(),
                },
                name: {
                    type: String,
                    trim: true,
                },
                payload: {
                    type: mongoose.Schema.Types.Mixed,
                },
                timeTaken: {
                    type: Number,
                    default: 0,
                },
                output: {
                    type: mongoose.Schema.Types.Mixed,
                },
                errors: {
                    type: mongoose.Schema.Types.Mixed,
                    default: {},
                },
                /*
                  Payload examples by type:
                  - email: { 
                      templateId: ObjectId,
                      to: "email@example.com" | "{{admin_email}}",
                      cc: [],
                      bcc: [],
                      subject: "Subject with {{variables}}",
                      content: "<html>...</html>",
                      variables: { key: value }
                    }
                  - webhook: { 
                      url: "https://api.example.com/hook",
                      method: "POST" | "GET" | "PUT",
                      headers: { Authorization: "Bearer ..." },
                      body: { data: "{{entityData}}" }
                    }
                  - job: { 
                      name: "job_name",
                      data: { ... },
                      delay: 0,
                      priority: "normal" | "high" | "low"
                    }
                  - db: { 
                      collection: "ModelName",
                      operation: "insertOne" | "updateOne" | "deleteOne",
                      filter: { field: "value" },
                      data: { field: "value" }
                    }
                */
            },
        ],
        /**
         * EXECUTION CONTROL
         * Settings for retry and timeout behavior
         */
        execution: {
            maxRetries: {
                type: Number,
                default: 3,
                min: 0,
                max: 10,
            },
            retryDelaySec: {
                type: Number,
                default: 60,
                min: 1,
            },
            timeoutSec: {
                type: Number,
                default: 30,
                min: 5,
                max: 300,
            },
        },
        /**
         * RUNTIME STATS
         * Tracking execution history
         */
        stats: {
            lastRunAt: {
                type: Date,
            },
            nextRunAt: {
                type: Date,
                index: true,
            },
            successCount: {
                type: Number,
                default: 0,
            },
            failureCount: {
                type: Number,
                default: 0,
            },
            lastError: {
                type: String,
            },
            lastExecutionDetails: {
                type: mongoose.Schema.Types.Mixed,
                default: [],
            },
        },
        createdBy: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User",
            resolveBy: "username",
            required: true,
        },
        /**
         * Owner of the trigger
         */
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User",
            resolveBy: "username",
            required: true,
        },
        /**
         * Soft delete flag
         */
        isDeleted: {
            type: Boolean,
            default: false,
            index: true,
        },
    },
    { timestamps: true }
);
const Trigger = mongoose.model("Trigger", TriggerSchema);
module.exports = Trigger;

```
