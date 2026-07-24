```javascript
// File: ./features/icon/icon.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const IconSchema = new Schema(
    {
        label: {
            type: String,
            required: true,
            trim: true
        },
        value: {
            type: String,
            required: true,
            trim: true
        }
    },
    {
        timestamps: false,
    }
);
const Icon = mongoose.model("Icon", IconSchema);
module.exports = Icon;

```
