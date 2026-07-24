```javascript
// File: ./features/vehicle-route/vehicle-route.model
const mongoose = require("mongoose");
const stopSchema = new mongoose.Schema({
    name: { type: String, required: true },
    sequence: { type: Number, required: true }, // Stop order
    pickupTime: { type: String }, // "07:30"
    dropTime: { type: String },
    location: {
        type: { type: String, enum: ["Point"], default: "Point" },
        coordinates: {
            type: [Number], // [lng, lat]
            default: [0, 0],
            validate: {
                validator: (v) => !v || (Array.isArray(v) && v.length === 2),
                message: "location.coordinates must be [lng, lat]",
            },
        },
    },
});
const vehicleRouteSchema = new mongoose.Schema({
    name: { type: String, required: true }, // Example: "North Zone Route"
    stops: [stopSchema],
    status: {
        type: String,
        enum: ["active", "inactive"],
        default: "active",
    },
    isDeleted: { type: Boolean, default: false },
    user: {
        type: mongoose.Schema.Types.ObjectId, ref: "User", resolveBy: "username", required: true
    }
}, { timestamps: true });
module.exports = mongoose.model("VehicleRoute", vehicleRouteSchema);

```
