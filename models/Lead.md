```javascript
// File: ./features/lead/lead.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
const Nationality = require("../../data/Nationality");
const Religions = require("../../data/Religions");
const Categories = require("../../data/Categories");
const Gender = require("../../data/Gender");
const { addressSchema } = require("../address/address.model");
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
    officialAddress: {
      type: addressSchema,
      name: "Father's Official Address",
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
    officialAddress: {
      type: addressSchema,
      name: "Mother's Official Address",
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
      name: "Mother's Email",
    },
    phone: {
      type: String,
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
    officialAddress: {
      type: addressSchema,
      name: "Guardian's Official Address",
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
      name: "Guardian's Email",
    },
    phone: {
      type: String,
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
const leadSchema = new mongoose.Schema(
  {
    course: {
      type: mongoose.Schema.Types.ObjectId,
      ref: 'Course',
      resolveBy: 'title',
      required: true,
      name: 'Course',
    },
    source: {
      type: String,
      name: 'Source'
    },
    referredBy: {
      type: String,
      dynamicref: "User", resolveBy: "username",
      name: 'Referred By'
    },
    avatar: {
      type: String,
      name: 'Avatar URL'
    },
    firstName: {
      type: String,
      name: 'First Name'
    },
    lastName: {
      type: String, name: 'Last Name'
    },
    email: {
      type: String, match: /.+@.+\..+/, name: 'Email',
    },
    phone: {
      type: String,
      name: 'Phone Number',
    },
    gender: {
      type: String, enum: Object.values(Gender), name: 'Gender'
    },
    dateOfBirth: {
      type: Date, name: 'Date of Birth'
    },
    mother: {
      type: motherSchema, name: 'Mother Details'
    },
    father: {
      type: fatherSchema, name: 'Father Details'
    },
    guardian: {
      type: guardianSchema, name: 'Guardian Details'
    },
    address: {
      type: addressSchema,
      name: "Address"
    },
    nationality: {
      type: String, enum: Object.values(Nationality), default: Nationality.INDIAN, name: 'Nationality'
    },
    religion: {
      type: String, enum: Object.values(Religions), default: Religions.HINDU, name: 'Religion'
    },
    category: {
      type: String, enum: Object.values(Categories), default: Categories.OBC, name: 'Category'
    },
    documents: {
      type: [documentSchema],
      default: [],
      name: "Documents",
    },
    qualifications: { type: [qualificationSchema], name: 'Qualifications' },
    fields: [
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
    scheduledAt: {
      type: Date,
    },
    status: {
      type: String,
    },
    remark: {
      type: String,
      trim: true,
    },
    employee: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
    },
    isDeleted: {
      type: Boolean,
      default: false
    },
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
  },
  { timestamps: true, toJSON: { virtuals: true }, toObject: { virtuals: true } }
);
const Lead = mongoose.model("Lead", leadSchema);
module.exports = Lead;

```
