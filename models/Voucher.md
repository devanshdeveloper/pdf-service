```javascript
// File: ./features/voucher/voucher.model
const mongoose = require("mongoose");
const PaymentMethods = require("../../data/PaymentMethods");
const { Schema } = mongoose;
const { VoucherTypesArray, voucherTypes, InvoiceStatusTypesArray, VoucherStatusTypesArray } = require("./voucher");
const VoucherSchema = new Schema(
  {
    number: {
      type: String,
      required: true,
      trim: true,
      name: "Voucher Number",
    },
    currency: {
      type: String,
      required: true,
      name: "Currency",
    },
    exchangeRate: {
      type: Number,
      required: true,
      default: 1,
      name: "Exchange Rate",
    },
    taxType: {
      type: String,
      enum: ["CGST_SGST", "IGST"],
      name: "Tax Type",
    },
    placeOfSupplyStateCode: {
      type: String,
      name: "Place of Supply State Code",
    },
    sellerStateCode: {
      type: String,
      name: "Seller State Code",
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User",
      required: true,
      name: "User",
      resolveBy: "email",
    },
    business: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Business",
      required: false, // Not required yet for backwards compatibility
      name: "Business",
    },
    party: {
      type: Schema.Types.ObjectId,
      ref: "Party",
      required: true,
      name: "Party",
    },
    consignee: {
      type: Schema.Types.ObjectId,
      ref: "Party",
      name: "Consignee",
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
    referenceDate: {
      type: Date,
      name: "Reference Date",
    },
    deliveryNote: {
      type: String,
      trim: true,
      name: "Delivery Note",
    },
    deliveryNoteDate: {
      type: Date,
      name: "Delivery Note Date",
    },
    buyerOrderNo: {
      type: String,
      trim: true,
      name: "Buyer's Order No.",
    },
    buyerOrderDate: {
      type: Date,
      name: "Buyer's Order Date",
    },
    dispatchDocNo: {
      type: String,
      trim: true,
      name: "Dispatch Doc No.",
    },
    dispatchedThrough: {
      type: String,
      trim: true,
      name: "Dispatched Through",
    },
    destination: {
      type: String,
      trim: true,
      name: "Destination",
    },
    termsOfDelivery: {
      type: String,
      trim: true,
      name: "Terms of Delivery",
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
        name: {
          type: String,
          trim: true,
          name: "Product Name",
        },
        hsn: {
          type: String,
          trim: true,
          name: "HSN/SAC Code",
        },
        quantity: {
          type: Number,
          required: true,
          min: 0,
          name: "Quantity",
        },
        price: {
          type: Number,
          min: 0,
          name: "Price",
        },
        unit: {
          type: Schema.Types.ObjectId,
          ref: "Unit",
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
          default: 0,
          name: "Discount Value",
        },
        discountAmount: {
          type: Number,
          min: 0,
          default: 0,
          name: "Discount Amount",
        },
        // Taxes
        taxes: [
          {
            name: {
              type: String,
              trim: true,
            },
            type: {
              type: String,
              enum: ["fixed", "percentage"],
              default: "percentage",
            },
            value: {
              type: Number,
            },
            taxCategory: {
              type: String,
              enum: ["cgst", "sgst", "igst", "cess", "other"],
              name: "Tax Category",
            },
            amount: {
              type: Number,
              min: 0,
              default: 0,
              name: "Tax Amount",
            },
          }
        ],
        taxAmount: {
          type: Number,
          min: 0,
          default: 0,
          name: "Tax Amount",
        },
        taxableAmount: {
          type: Number,
          min: 0,
          default: 0,
          name: "Taxable Amount",
        },
        amount: {
          type: Number,
          min: 0,
          default: 0,
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
        type: {
          type: String,
          enum: ["fixed", "percentage"],
          default: "fixed",
          name: "Cost Type",
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
      default: 0,
      name: "Discount Value",
    },
    couponCode: {
      type: String,
      trim: true,
      name: "Coupon Code",
    },
    signature: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "Signature",
      trim: true,
      name: "Signature",
    },
    status: {
      type: String,
      enum: VoucherStatusTypesArray,
      default: "pending",
      name: "Status",
    },
    paymentStatus: {
      type: String,
      enum: InvoiceStatusTypesArray,
      default: "unpaid",
      name: "Payment Status",
    },
    // Computed/Summary Fields for Indian Invoice Requirements
    subtotal: {
      type: Number,
      min: 0,
      default: 0,
      name: "Subtotal",
    },
    totalDiscountAmount: {
      type: Number,
      min: 0,
      default: 0,
      name: "Total Discount",
    },
    totalTaxAmount: {
      type: Number,
      min: 0,
      default: 0,
      name: "Total Tax",
    },
    totalCostAmount: {
      type: Number,
      min: 0,
      default: 0,
      name: "Total Additional Cost",
    },
    grandTotal: {
      type: Number,
      min: 0,
      default: 0,
      name: "Grand Total",
    },
    roundOff: {
      type: Number,
      default: 0,
      name: "Round Off",
    },
    amountInWords: {
      type: String,
      trim: true,
      name: "Amount in Words",
    },
    type: {
      type: String,
      enum: VoucherTypesArray,
      default: voucherTypes.Voucher,
      name: "Type",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
      name: "User",
    },
    nextVoucher: {
      type: Schema.Types.ObjectId,
      ref: "Voucher",
      name: "Next Voucher",
    },
    previousVoucher: {
      type: Schema.Types.ObjectId,
      ref: "Voucher",
      name: "Previous Voucher",
    },
    nextMaterialEntry: {
      type: Schema.Types.ObjectId,
      ref: "MaterialEntry",
      name: "Next Material Entry",
    },
    previousMaterialEntry: {
      type: Schema.Types.ObjectId,
      ref: "MaterialEntry",
      name: "Previous Material Entry",
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
const Voucher = mongoose.model("Voucher", VoucherSchema);
module.exports = Voucher;

```
