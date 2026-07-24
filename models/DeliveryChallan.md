```javascript
// File: ./features/delivery-challan/delivery-challan.model
const mongoose = require("mongoose");
const PaymentMethods = require("../../data/PaymentMethods");
const { Schema } = mongoose;
const DeliveryChallanSchema = new Schema(
  {
    number: {
      type: String,
      required: true,
      trim: true,
      name: "Challan Number",
    },
    customer: {
      type: Schema.Types.ObjectId,
      ref: "Customer",
      required: true,
      name: "Customer",
    },
    date: {
      type: Date,
      required: true,
      name: "Date",
    },
    dueDate: {
      type: Date,
      required: true,
      name: "Due Date",
    },
    referenceNumber: {
      type: String,
      trim: true,
      name: "Reference Number",
    },
    paymentMethod: {
      type: String,
      enum: Object.values(PaymentMethods),
      name: "Payment Method",
    },
    address: {
      type: String,
      trim: true,
      name: "Address",
    },
    products: [
      {
        product: {
          type: Schema.Types.ObjectId,
          ref: "Product",
          required: true,
          name: "Product",
        },
        quantity: {
          type: Number,
          required: true,
          min: 0,
          name: "Quantity",
        },
        returnQuantity: {
          type: Number,
          min: 0,
          name: "Return Quantity",
        },
        price: {
          type: Number,
          // required: true,
          min: 0,
          name: "Price",
        },
        unit: {
          type: Schema.Types.ObjectId,
          ref: "Unit",
          // required: true,
          name: "Unit",
        },
        discountType: {
          type: String,
          enum: ["fixed", "percentage"],
          default: "fixed",
          name: "Discount Type",
        },
        discountValue: {
          type: Number,
          min: 0,
          name: "Discount",
        },
        taxes: [
          {
            type: Schema.Types.ObjectId,
            ref: "Tax",
            required: true,
            name: "Tax",
          }
        ],
        amount: {
          type: Number,
          // required: true,
          min: 0,
          name: "Amount",
        }
      },
    ],
    bank: {
      type: Schema.Types.ObjectId,
      ref: "Bank",
      name: "Bank",
    },
    notes: {
      type: String,
      trim: true,
      name: "Notes",
    },
    terms: {
      type: String,
      trim: true,
      name: "Terms",
    },
    cost: [
      {
        name: {
          type: String,
          required: true,
          trim: true,
          name: "Cost Name",
        },
        value: {
          type: Number,
          required: true,
          min: 0,
          name: "Cost Value",
        },
      },
    ],
    discountType: {
      type: String,
      enum: ["fixed", "percentage"],
      default: "fixed",
      name: "Discount Type",
    },
    discountValue: {
      type: Number,
      min: 0,
      name: "Discount",
    },
    couponCode: {
      type: String,
      trim: true,
      name: "Coupon Code",
    },
    signature: {
      type: mongoose.Schema.Types.ObjectId, // Could be a URL or Base64 string
      ref: "Signature",
      trim: true,
      name: "Signature",
    },
    status: {
      type: String,
      enum: ["pending", "approved", "return-approved", "rejected"],
      default: "pending",
      name: "Status",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "User",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
    },
  },
  {
    timestamps: true,
  }
);
const DeliveryChallan = mongoose.model("DeliveryChallan", DeliveryChallanSchema);
module.exports = DeliveryChallan;

```
