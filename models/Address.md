```javascript
// File: ./features/address/address.model
const mongoose = require("mongoose");
function buildFullAddress(addr) {
  return [
    addr.streetAddress,
    addr.apartment,
    addr.city,
    addr.state,
    addr.postalCode,
    addr.country,
  ]
    .filter(Boolean)
    .join(", ");
}
const addressSchema = new mongoose.Schema(
  {
    streetAddress: {
      type: String,
      trim: true,
      name: "Street Address"
    },
    apartment: {
      type: String,
      trim: true,
      maxlength: [20, "Apartment/Suite number cannot exceed 20 characters"],
      name: "Apartment/Suite"
    },
    city: {
      type: String,
      trim: true,
      maxlength: [50, "City name cannot exceed 50 characters"],
      name: "City"
    },
    district: {
      type: String,
      trim: true,
      maxlength: [50, "District name cannot exceed 50 characters"],
      name: "District"
    },
    state: {
      type: String,
      trim: true,
      maxlength: [50, "State name cannot exceed 50 characters"],
      name: "State"
    },
    stateCode: {
      type: String,
      trim: true,
      maxlength: [2, "State code cannot exceed 2 characters"],
      name: "State Code"
    },
    postalCode: {
      type: String,
      trim: true,
      name: "Postal Code"
    },
    geo: {
      lat: {
        type: Number,
        trim: true,
        name: "Latitude"
      },
      lng: {
        type: Number,
        trim: true,
        name: "Longitude"
      }
    },
    country: {
      type: String,
      trim: true,
      maxlength: [50, "Country name cannot exceed 50 characters"],
      name: "Country"
    },
  },
  {
    _id: false,
    timestamps: false,
    toJSON: {
      transform(doc, ret) {
        ret.fullAddress = buildFullAddress(ret);
        return ret;
      },
    },
    toObject: {
      transform(doc, ret) {
        console.log("Transforming address to object:", ret);
        ret.fullAddress = buildFullAddress(ret);
        return ret;
      },
    },
  }
);
const Address = mongoose.model("Address", addressSchema);
module.exports = { default: Address, addressSchema };

```
