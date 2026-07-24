```javascript
// File: ./features/vehicle/vehicle.model
const mongoose = require("mongoose");
const documentSchema = new mongoose.Schema({
    label: {
        type: String,
        name: "Document Label",
    },
    value: {
        type: String,
        name: "Document URL",
    },
    identifier: {
        type: String,
        name: "Document Identifier"
    },
    issuedOn: { type: Date, default: Date.now },
    expiryOn: { type: Date },
}, { _id: false });
const maintenanceSchema = new mongoose.Schema({
    date: { type: Date, default: Date.now },
    description: { type: String },
    cost: { type: Number, default: 0 },
    party: { type: mongoose.Schema.Types.ObjectId, ref: "Party" },
    nextDueOn: { type: Date },
}, { _id: false });
const vehicleSchema = new mongoose.Schema({
    name: { type: String, required: true }, // e.g., "School Bus 1"
    vehicleType: { type: String, enum: ["bus", "van", "car", "tempo", "others"], default: "bus" },
    model: { type: String, required: true },
    make: { type: String }, // Manufacturer
    year: {
        type: Number,
        required: true,
        min: 1950,
    },
    color: { type: String },
    registrationNumber: { type: String, required: true, unique: true }, // unique VIN/RTO reg no.
    chassisNumber: { type: String },
    engineNumber: { type: String },
    assignedRoutes: [{
        type: mongoose.Schema.Types.ObjectId, ref: "VehicleRoute"
    }],
    driver: { type: mongoose.Schema.Types.ObjectId, ref: "Employee", resolveBy: "username" },   // driver staff record
    conductor: { type: mongoose.Schema.Types.ObjectId, ref: "Employee", resolveBy: "username" }, // optional
    seatingCapacity: { type: Number, default: 0 },
    wheelchairAccessible: { type: Boolean, default: false },
    fuelType: { type: String, enum: ["diesel", "petrol", "cng", "electric", "hybrid", "other"], default: "diesel" },
    documents: [documentSchema],
    status: { type: String, enum: ["active", "inactive", "under_maintenance"], default: "active" },
    gpsDeviceId: { type: String }, // tracker id if integrated
    maintenanceRecords: [maintenanceSchema],
    lastServicedOn: { type: Date },
    emergencyContact: {
        name: { type: String },
        phone: { type: String },
        relation: { type: String }
    },
    isDeleted: { type: Boolean, default: false, },
    createdBy: { type: mongoose.Schema.Types.ObjectId, ref: "User" },
    updatedBy: { type: mongoose.Schema.Types.ObjectId, ref: "User" },
    user: {
        type: mongoose.Schema.Types.ObjectId, ref: "User", resolveBy: "username",
        required: true
    }
}, { timestamps: true });
module.exports = mongoose.model("Vehicle", vehicleSchema);

```
