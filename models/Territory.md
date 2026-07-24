```javascript
// File: ./features/territory/territory.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
/**
 * Territory Status Enum
 */
const TerritoryStatus = Object.freeze({
    PENDING: "pending",
    ACTIVE: "active",
    INACTIVE: "inactive",
    SUSPENDED: "suspended",
});
/**
 * Territory Level Enum
 */
const TerritoryLevel = Object.freeze({
    COUNTRY: "country",
    STATE: "state",
    CITY: "city",
    DISTRICT: "district",
    ZONE: "zone",
});
/**
 * Territory Schema
 */
const territorySchema = new Schema(
    {
        name: {
            type: String,
            required: true,
            trim: true,
            index: true,
            name: "Territory Name",
        },
        code: {
            type: String,
            required: true,
            trim: true,
            uppercase: true,
            unique: true,
            index: true,
            name: "Territory Code",
        },
        level: {
            type: String,
            enum: Object.values(TerritoryLevel),
            required: true,
            index: true,
            name: "Territory Level",
        },
        parent: {
            type: Schema.Types.ObjectId,
            ref: "Territory",
            index: true,
            name: "Parent Territory",
        },
        status: {
            type: String,
            enum: Object.values(TerritoryStatus),
            default: TerritoryStatus.PENDING,
            index: true,
            name: "Territory Status",
        },
        notes: {
            type: String,
            trim: true,
            name: "Internal Notes",
        },
        isDeleted: {
            type: Boolean,
            default: false,
            index: true,
            name: "Is Deleted",
        },
        createdBy: {
            type: Schema.Types.ObjectId,
            ref: "User",
            index: true,
            name: "Created By",
        },
    },
    {
        timestamps: true,
    }
);
module.exports = mongoose.model("Territory", territorySchema);

```
