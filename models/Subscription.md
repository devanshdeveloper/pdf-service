```javascript
// File: ./features/subscription/subscription.model
const mongoose = require("mongoose");
const { BillingCycleEnum } = require("../plan/constants/BillingCycleEnum");
const SubscriptionSchema = new mongoose.Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      minlength: 2,
      maxlength: 50,
    },
    description: {
      type: String,
      required: true,
      trim: true,
      maxlength: 500,
    },
    price: {
      type: Number,
      required: true,
      min: 0,
    },
    amount: {
      type: Number,
      required: true,
      min: 1,
    },
    currency: {
      type: String,
      required: true,
      trim: true,
      maxlength: 5,
      default: "INR",
    },
    billingCycle: {
      type: String,
      enum: Object.values(BillingCycleEnum),
      required: true,
      description: "Billing cycle",
    },
    trialDays: {
      type: Number,
      required: true,
      min: 1,
      description: "Trial in days",
    },
    features: [
      {
        name: {
          type: String,
          required: true,
        },
        description: String,
        enabled: {
          type: Boolean,
          default: true,
        },
      },
    ],
    startDate: {
      type: Date,
      required: true,
      default: Date.now,
    },
    endDate: {
      type: Date,
      required: true,
    },
    plan: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Plan",
      required: true,
    },
    status: {
      type: String,
      enum: ["active", "inactive", "cancelled", "renewed"],
      default: "active",
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    permissions: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Permission",
      name: "Permissions",
    },
  },
  { timestamps: true }
);
const Subscription = mongoose.model("Subscription", SubscriptionSchema);
module.exports = Subscription;

```
