```javascript
// File: ./features/settings/customer/customer-settings.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const customerSettingSchema = new Schema(
  {
    templates: {
      username: {
        type: String,
        required: true,
        trim: true,
        minlength: 2,
        maxlength: 50,
        default: "{{firstName}}{{lastName}}",
      },
      password: {
        type: String,
        required: true,
        trim: true,
        minlength: 2,
        maxlength: 50,
        default: "{{firstName}}{{birthYear}}",
      },
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  {
    timestamps: true,
  }
);
const CustomerSetting = mongoose.model("CustomerSetting", customerSettingSchema);
module.exports = CustomerSetting;

```
