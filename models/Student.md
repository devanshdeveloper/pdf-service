```javascript
// File: ./features/student/student.model
const mongoose = require("mongoose");
const { addressSchema } = require("../address/address.model");
const Gender = require("../../data/Gender");
const Nationality = require("../../data/Nationality");
const Religions = require("../../data/Religions");
const Categories = require("../../data/Categories");
const { Schema } = mongoose;
const AdmissionTypeEnum = Object.freeze({
  New: "New",
  Old: "Old",
});
// Qualification sub-schema
const qualificationSchema = new Schema(
  {
    qualification: {
      type: String,
      name: "Qualification",
    },
    passYear: {
      type: Number,
      min: [1950, "Pass year cannot be earlier than 1950"],
      max: [new Date().getFullYear(), "Pass year cannot be in the future"],
      name: "Pass Year",
    },
    percentage: {
      type: Number,
      min: [0, "Percentage cannot be less than 0"],
      max: [100, "Percentage cannot be more than 100"],
      name: "Percentage",
    },
    institute: {
      type: String,
      name: "Institute/School",
    },
  },
  { _id: false }
);
// Document sub-schema
const documentSchema = new Schema(
  {
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
    }
  },
  { _id: false }
);
// Email validator
function validateEmail(email) {
  if (!email) return false;
  const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  return emailRegex.test(email);
}
function validatePhone(phone) {
  if (!phone) return false;
  const phoneRegex = /^[6-9]\d{9}$/;
  return phoneRegex.test(phone);
}
// Parent sub-schemas
const fatherSchema = new Schema(
  {
    firstName: {
      type: String,
      name: "Father's First Name",
    },
    lastName: {
      type: String,
      name: "Father's Last Name",
    },
    qualification: {
      type: String,
      name: "Father's Qualification",
    },
    address: {
      type: String,
      name: "Father's Residential Address",
    },
    occupation: {
      type: String,
      name: "Father's Occupation",
    },
    annualIncome: {
      type: Number,
      min: [0, "Annual income cannot be negative"],
      max: [1000000000, "Annual income seems too high"],
      name: "Father's Annual Income",
    },
    email: {
      type: String,
      validate: [validateEmail, "Invalid email"],
      name: "Father's Email",
    },
    phone: {
      type: String,
      name: "Father's Phone Number",
    },
    userId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: 'User', resolveBy: "username",
      name: 'User ID'
    },
    createAccount: {
      type: Boolean,
      default: false,
      name: 'Create Account'
    }
  },
  { _id: false }
);
const motherSchema = new Schema(
  {
    firstName: {
      type: String,
      name: "Mother's First Name",
    },
    lastName: {
      type: String,
      name: "Mother's Last Name",
    },
    qualification: {
      type: String,
      name: "Mother's Qualification",
    },
    address: {
      type: String,
      name: "Mother's Residential Address",
    },
    occupation: {
      type: String,
      name: "Mother's Occupation",
    },
    annualIncome: {
      type: Number,
      min: [0, "Annual income cannot be negative"],
      max: [1000000000, "Annual income seems too high"],
      name: "Mother's Annual Income",
    },
    email: {
      type: String,
      validate: [validateEmail, "Invalid email"],
      name: "Mother's Email",
    },
    phone: {
      type: String,
      validate: [validatePhone, "Please enter a valid 10-digit mobile number"],
      name: "Mother's Phone Number",
    },
    userId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: 'User', resolveBy: "username",
      name: 'User ID'
    },
    createAccount: {
      type: Boolean,
      default: false,
      name: 'Create Account'
    }
  },
  { _id: false }
);
const guardianSchema = new Schema(
  {
    firstName: {
      type: String,
      name: "Guardian's First Name",
    },
    lastName: {
      type: String,
      name: "Guardian's Last Name",
    },
    qualification: {
      type: String,
      name: "Guardian's Qualification",
    },
    address: {
      type: String,
      name: "Guardian's Residential Address",
    },
    occupation: {
      type: String,
      name: "Guardian's Occupation",
    },
    annualIncome: {
      type: Number,
      min: [0, "Annual income cannot be negative"],
      max: [1000000000, "Annual income seems too high"],
      name: "Guardian's Annual Income",
    },
    email: {
      type: String,
      validate: [validateEmail, "Invalid email"],
      name: "Guardian's Email",
    },
    phone: {
      type: String,
      validate: [validatePhone, "Please enter a valid 10-digit mobile number"],
      name: "Guardian's Phone Number",
    },
    userId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: 'User', resolveBy: "username",
      name: 'User ID'
    },
    createAccount: {
      type: Boolean,
      default: false,
      name: 'Create Account'
    }
  },
  { _id: false }
);
const studentSchema = new Schema(
  {
    session: {
      type: String,
      match: [/^\d{4}-\d{4}$/, "Session must be in format YYYY-YYYY"],
      name: "Session",
    },
    admissionDate: {
      type: Date,
      validate: {
        validator: function (value) {
          return value <= new Date();
        },
        message: "Admission date cannot be in the future",
      },
      name: "Admission Date",
    },
    course: {
      type: Schema.Types.ObjectId,
      ref: "Course",
      resolveBy: "title",
      name: "Course",
    },
    classroom: {
      type: Schema.Types.ObjectId,
      ref: "Classroom",
      resolveBy: "name",
      name: "Classroom",
    },
    department: {
      type: Schema.Types.ObjectId,
      ref: "Department",
      resolveBy: "name",
      name: "Department",
    },
    identifiers: {
      type: [
        {
          label: {
            type: String,
            trim: true,
            minlength: [2, "Identifier label must be at least 2 characters"],
            maxlength: [50, "Identifier label cannot exceed 50 characters"],
          },
          value: {
            type: String,
            trim: true,
            maxlength: [100, "Identifier value cannot exceed 100 characters"],
          },
        },
      ],
      name: "Identifiers",
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
    father: {
      type: fatherSchema,
      name: "Father Details",
    },
    mother: {
      type: motherSchema,
      name: "Mother Details",
    },
    guardian: {
      type: guardianSchema,
      name: "Guardian Details",
    },
    nationality: {
      type: String,
      enum: Object.values(Nationality),
      default: Nationality.INDIAN,
      name: "Nationality",
    },
    religion: {
      type: String,
      enum: Object.values(Religions),
      name: "Religion",
    },
    category: {
      type: String,
      enum: Object.values(Categories),
      name: "Category",
    },
    address: {
      type: mongoose.Schema.Types.Mixed,
      name: "Address",
    },
    documents: {
      type: [documentSchema],
      default: [],
      name: "Documents",
    },
    qualifications: {
      type: [qualificationSchema],
      default: [],
      name: "Qualifications",
    },
    admissionType: {
      type: String,
      enum: Object.values(AdmissionTypeEnum),
      default: AdmissionTypeEnum.New,
      name: "Admission Type",
    },
    username: {
      type: String,
      unique: true,
      trim: true,
      sparse: true,
      name: "Username",
    },
    password: {
      type: String,
      name: "Password",
    },
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
module.exports = mongoose.model("Student", studentSchema);

```
