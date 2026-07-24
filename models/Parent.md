```javascript
// File: ./features/parent/parent.model
const mongoose = require("mongoose");
const Gender = require("../../data/Gender");
const { Schema } = mongoose;
// Email validator
function validateEmail(email) {
  const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  return emailRegex.test(email);
}
function validatePhone(phone) {
  const phoneRegex = /^[6-9]\d{9}$/;
  return phoneRegex.test(phone);
}
const parentSchema = new Schema(
  {
    session: {
      type: String,
      match: [/^\d{4}-\d{4}$/, "Session must be in format YYYY-YYYY"],
      name: "Session",
    },
    avatar: {
      type: String,
      trim: true,
      name: "Avatar URL",
    },
    firstName: {
      type: String,
      name: "First Name",
    },
    lastName: {
      type: String,
      name: "Last Name",
    },
    email: {
      type: String,
      validate: [validateEmail, "Invalid email"],
      name: "Email",
    },
    phone: {
      type: String,
      validate: [validatePhone, "Please enter a valid 10-digit mobile number"],
      name: "Phone Number",
    },
    dateOfBirth: {
      type: Date,
      name: "Date of Birth",
    },
    gender: {
      type: String,
      enum: Object.values(Gender),
      name: "Gender",
    },
    qualification: {
      type: String,
      name: "Parent's Qualification",
    },
    address: {
      type: String,
      name: "Parent's Residential Address",
    },
    occupation: {
      type: String,
      name: "Parent's Occupation",
    },
    students: [
      {
        type: Schema.Types.ObjectId,
        ref: "Student",
        resolveBy: "username",
        name: "Students",
      }
    ],
    username: {
      type: String,
      unique: true,
      trim: true,
      lowercase: true,
      maxlength: [30, "Username cannot exceed 30 characters"],
      name: "Username",
    },
    password: {
      type: String,
      name: "Password",
    },
    userId: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      name: "User ID",
    },
    isDeleted: {
      type: Boolean,
      default: false,
      name: "Is Deleted",
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      name: "User",
      required: true,
    },
  },
  { timestamps: true }
);
module.exports = mongoose.model("Parent", parentSchema);

```
