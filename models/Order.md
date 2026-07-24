```javascript
// File: ./features/order/order.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");
const PaymentMethods = require("../../data/PaymentMethods");
const productSubSchema = new mongoose.Schema(
    {
        product: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "Product",
            required: true,
            name: "Product",
        },
        quantity: {
            type: Number,
            name: "Quantity",
            required: true,
            min: 1,
        },
        price: {
            type: Number,
            name: "Price",
            required: true,
            min: 0,
        },
    },
    { _id: false }
);
const orderSchema = new mongoose.Schema(
    {
        products: {
            type: [productSubSchema],
            name: "Products",
            validate: [(val) => val.length > 0, "Order must have at least one product"],
        },
        paymentMethod: {
            type: String,
            name: "Payment Method",
            enum: Object.values(PaymentMethods),
            required: true,
        },
        shippingAddress: {
            type: addressSchema,
            name: "Shipping Address",
            required: true,
        },
        userId: {
            // customer id
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
        user: {
            type: mongoose.Schema.Types.ObjectId,
            ref: "User", resolveBy: "username",
            required: true,
        },
        isDeleted: {
            type: Boolean,
            default: false,
        },
    },
    { timestamps: true }
);
const Order = mongoose.model("Order", orderSchema);
module.exports = Order;

```
