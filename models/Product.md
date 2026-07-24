```javascript
// File: ./features/product/ProductHelper
const { ModelHelper } = require("../../helpers/ModelHelper");
const AggregationBuilder = require("../../helpers/aggregation/AggregationBuilder");
const { Types } = require("mongoose");
class ProductHelper extends ModelHelper {
    constructor(model) {
        super(model);
    }
    async paginate(filter = {}, options = {}) {
        const { select = "", context = {} } = options;
        const requestedFields = select?.split?.(" ") || [];
        const computedFields = [
            "reviews_count",
            "sales",
            "ordered_count",
            "favourite",
            "cart_quantity"
        ];
        const hasComputedFields = computedFields.some(field => requestedFields.includes(field));
        if (!hasComputedFields) {
            return super.paginate(filter, options);
        }
        const userId = context.user?.user || context.user?._id;
        const userObjectId = userId ? new Types.ObjectId(userId) : null;
        const builder = new AggregationBuilder();
        builder
            .castFilter(filter)
            // 1. Attach reviews_count
            .check(requestedFields.includes("reviews_count"), (b) => {
                b.lookupMatchCount({
                    from: "productreviews",
                    localField: "_id",
                    foreignField: "product",
                    as: "reviews_count_data"
                })
                    .extractCount("reviews_count", "reviews_count_data")
                    .project({ reviews_count_data: 0 });
            })
            // 2. Attach sales
            .check(requestedFields.includes("sales"), (b) => b
                .lookup({
                    from: "orders",
                    let: { productId: "$_id" },
                    pipeline: (sub) => sub
                        .unwind("$products")
                        .match({ $expr: { $eq: ["$products.product", "$$productId"] }, status: "completed" })
                        .group({ _id: null, totalSales: { $sum: "$products.quantity" } }),
                    as: "sales_data"
                })
                .add({
                    sales: { $ifNull: [{ $arrayElemAt: ["$sales_data.totalSales", 0] }, 0] }
                })
                .project({ sales_data: 0 })
            )
            // 3. Attach ordered_count
            .check(requestedFields.includes("ordered_count") && !!userObjectId, (b) => b
                .lookup({
                    from: "orders",
                    let: { productId: "$_id" },
                    pipeline: (sub) => sub
                        .match({ $expr: { $eq: ["$user", userObjectId] } })
                        .unwind("$products")
                        .match({ $expr: { $eq: ["$products.product", "$$productId"] } })
                        .group({ _id: null, count: { $sum: "$products.quantity" } }),
                    as: "ordered_count_data"
                })
                .extractCount("ordered_count", "ordered_count_data")
                .project({ ordered_count_data: 0 })
            )
            // 4. Attach favourite
            .check(requestedFields.includes("favourite") && !!userObjectId, (b) => b
                .lookup({
                    from: "favouriteproducts",
                    let: { productId: "$_id" },
                    pipeline: (sub) => sub
                        .match({ $expr: { $and: [{ $eq: ["$product", "$$productId"] }, { $eq: ["$user", userObjectId] }] } })
                        .limit(1),
                    as: "favourite_data"
                })
                .add({
                    favourite: { $gt: [{ $size: "$favourite_data" }, 0] }
                })
                .project({ favourite_data: 0 })
            )
            // 5. cart_quantity
            .check(requestedFields.includes("cart_quantity") && !!userObjectId, (b) => b
                .lookup({
                    from: "carts",
                    let: { productId: "$_id" },
                    pipeline: (sub) => sub
                        .match({ $expr: { $eq: ["$user", userObjectId] } })
                        .unwind("$products")
                        .match({ $expr: { $eq: ["$products.product", "$$productId"] } })
                        .limit(1),
                    as: "cart_data"
                })
                .add({
                    cart_quantity: { $ifNull: [{ $arrayElemAt: ["$cart_data.products.quantity", 0] }, 0] }
                })
                .project({ cart_data: 0 })
            )
            // Sort
            .check(!!options.sort, (b) => b.sort(options.sort));
        const pipeline = builder.build();
        return this.paginatedAggregate(pipeline, {
            page: options.page,
            limit: options.limit
        });
    }
}
module.exports = ProductHelper;


// File: ./features/product/product.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const ProductSchema = new Schema(
  {
    // Product Details
    images: [
      {
        type: String,
      },
    ],
    // Note: images array doesn't need a name property
    name: {
      type: String,
      required: true,
      trim: true,
      name: "Product Name",
    },
    description: {
      type: String,
      trim: true,
      name: "Description",
    },
    slug: {
      type: String,
      trim: true,
      lowercase: true,
      name: "URL Slug"
    },
    sku: {
      type: String,
      trim: true,
      name: "SKU",
    },
    hsn: {
      type: String,
      trim: true,
      name: "HSN/SAC Code",
    },
    sortPriority: {
      type: Number,
      name: "Sort Priority",
    },
    views: {
      type: Number,
      name: "Product Views",
    },
    sales: {
      type: Number,
      name: "Product Sales",
    },
    purchasePrice: {
      type: Number,
      name: "Purchase Price",
    },
    price: {
      type: Number,
      name: "Price",
    },
    stock: {
      type: Number,
      name: "Opening Stock",
    },
    stockAlert: {
      type: Number,
      name: "Stock Alert",
    },
    category: {
      type: Schema.Types.ObjectId,
      ref: "Category",
      resolveBy: "name",
      name: "Category",
    },
    unit: {
      type: Schema.Types.ObjectId,
      ref: "Unit",
      trim: true,
      resolveBy: "name",
      name: "Unit",
    },
    featured: {
      type: Boolean,
      default: false,
      name: "Featured",
    },
    categories: [
      {
        type: Schema.Types.ObjectId,
        ref: "Category",
        resolveBy: "name",
        name: "Category",
      }
    ],
    openingStockLocation: {
      type: Schema.Types.ObjectId,
      ref: "InventoryLocation",
      resolveBy: "name",
      name: "Opening Stock Location",
    },
    productType: {
      type: String,
      enum: ["Goods", "Service"],
      default: "Goods",
      name: "Type",
    },
    preferredVendor: {
      type: Schema.Types.ObjectId,
      ref: "Party",
      resolveBy: "businessName",
      name: "Preferred Vendor",
    },
    // Price Ranges
    prices: [
      {
        min: {
          type: Number,
        },
        max: {
          type: Number,
        },
        price: {
          type: Number,
        },
      },
    ],
    // Variants
    variants: [
      {
        name: {
          type: String,
          trim: true,
        },
        value: {
          type: String,
          trim: true,
        },
        product: {
          type: Schema.Types.ObjectId,
          ref: "Product",
        }
      }
    ],
    // Taxes
    taxes: [
      {
        taxId: {
          type: Schema.Types.ObjectId,
          ref: "Tax",
        },
        name: {
          type: String,
          trim: true,
        },
        value: {
          type: Number,
        },
      }
    ],
    fields: {
      type: [
        {
          label: {
            type: String,
            required: true,
          },
          value: {
            type: Schema.Types.Mixed,
            required: true,
          },
        },
      ],
      name: "Custom Fields",
    },
    // Discounts
    discountType: {
      type: String,
      enum: ["fixed", "percentage"],
      default: "fixed",
      name: "Discount Type",
    },
    discountValue: {
      type: Number,
      name: "Discount Value",
    },
    hidden: {
      type: Boolean,
      default: false,
      name: "Is Hidden"
    },
    status: {
      type: String,
      enum: ["active", "inactive"],
      default: "active",
      name: "Status",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
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
const Product = mongoose.model("Product", ProductSchema);
module.exports = Product;

```
