```javascript
// File: ./features/plan/plan.model
const { Schema, model } = require("mongoose");
const { BillingCycleEnum } = require("./constants/BillingCycleEnum");
const planSchema = new Schema(
  {
    name: {
      type: String,
      required: true,
      trim: true,
      unique: true,
      name: "Plan Name",
    },
    description: {
      type: String,
      required: true,
      trim: true,
      name: "Description",
    },
    price: [
      {
        amount: {
          type: Number,
          required: true,
          min: 0,
          name: "Amount",
        },
        currency: {
          type: String,
          required: true,
          default: "INR",
          enum: ["USD", "EUR", "GBP", "INR"],
          name: "Currency",
        },
        billingCycle: {
          type: String,
          required: true,
          enum: [
            BillingCycleEnum.MONTHLY,
            BillingCycleEnum.QUARTERLY,
            BillingCycleEnum.YEARLY,
          ],
          name: "Billing Cycle",
        },
      },
    ],
    features: [
      {
        name: {
          type: String,
          required: true,
          name: "Feature Name",
        },
        description: {
          type: String,
          name: "Feature Description",
        },
        enabled: {
          type: Boolean,
          default: true,
          name: "Enabled",
        },
      },
    ],
    isActive: {
      type: Boolean,
      default: true,
      name: "Active Status",
    },
    trialDays: {
      type: Number,
      default: 0,
      min: 0,
      name: "Trial Period (Days)",
    },
    recommended: {
      type: Boolean,
      default: false,
      name: "Recommended Plan",
    },
    razorpay_plan_ids: [
      {
        type: String,
        unique: true,
        sparse: true,
        name: "Razorpay Plan ID",
      },
    ],
    discountIfReferred: {
      type: Number,
      min: 0,
      max: 100,
      name: "Discount If Referred",
    },
    commissionIfReferred: {
      type: Number,
      min: 0,
      max: 100,
      name: "Commission If Referred",
    },
    permissions: {
      type: Schema.Types.ObjectId,
      ref: "Permission",
      required: true,
      name: "Permissions",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Deleted Status",
    },
  },
  {
    timestamps: true,
  }
);
// Calculate normalized monthly price for sorting
planSchema.virtual("normalizedMonthlyPrice").get(function () {
  if (!this.price || this.price.length === 0) return -1;
  return Math.min(
    ...this.price.map((p) => {
      const amount = p.amount;
      switch (p.billingCycle) {
        case BillingCycleEnum.MONTHLY:
          return amount;
        case BillingCycleEnum.QUARTERLY:
          return amount / 3;
        case BillingCycleEnum.YEARLY:
          return amount / 12;
        default:
          return amount;
      }
    })
  );
});
const Plan = model("Plan", planSchema);
module.exports = Plan;

```
