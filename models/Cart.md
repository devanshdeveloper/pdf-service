```javascript
// File: ./features/cart/cart.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const CartSchema = new Schema(
  {
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    products: [
      {
        product: {
          type: Schema.Types.ObjectId,
          ref: "Product",
          required: true,
        },
        quantity: {
          type: Number,
          required: true,
          min: 1,
        },
      },
    ],
  },
  { timestamps: true }
);
const Cart = mongoose.model("Cart", CartSchema);
module.exports = Cart;
```
