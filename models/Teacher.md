```javascript
// File: ./features/teacher/Teacher.model
const mongoose = require('mongoose');
const { addressSchema } = require('../address/address.model');
const { bankSchema } = require('../party/party.model');
const { Schema } = mongoose;
// Enums
const GenderEnum = Object.freeze({
  Male: 'Male',
  Female: 'Female',
  Other: 'Other'
});
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
// Experience sub-schema
const experienceSchema = new Schema({
  organization: { type: String, name: 'Organization' },
  position: { type: String, name: 'Position' },
  years: { type: Number, name: 'Years of Experience' }
}, { _id: false });
// Qualification sub-schema
const qualificationSchema = new Schema({
  qualification: { type: String, required: true, name: 'Qualification' },
  college: { type: String, name: 'College' },
  passingYear: { type: Number, name: 'Passing Year' }
}, { _id: false });
// Email validator
function validateEmail(email) {
  return /.+@.+\..+/.test(email);
}
// Teacher schema
const teacherSchema = new Schema({
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
  joiningDate: {
    type: Date, required: true, name: 'Joining Date'
  },
  avatar: {
    type: String, name: 'Avatar URL'
  },
  firstName: {
    type: String,
    required: true, name: 'First Name',
  },
  lastName: {
    type: String,
    name: 'Last Name'
  },
  email: {
    type: String,
    validate: [validateEmail, 'Invalid email'],
    name: 'Email',
  },
  phone: {
    type: String,
    name: 'Mobile Number',
  },
  gender: {
    type: String,
    enum: Object.values(GenderEnum),
    name: 'Gender'
  },
  dateOfBirth: {
    type: Date,
    name: 'Date of Birth'
  },
  maritalStatus: {
    type: String,
    name: 'Marital Status'
  },
  spouseName: {
    type: String,
    name: "Spouse's Name"
  },
  fatherName: {
    type: String,
    name: "Father's Name"
  },
  address: {
    type: addressSchema,
    name: 'Address'
  },
  nationality: {
    type: String,
    default: 'INDIAN',
    name: 'Nationality'
  },
  religion: {
    type: String, name: 'Religion'
  },
  category: {
    type: String, name: 'Category'
  },
  experiences: {
    type: [experienceSchema], name: 'Experiences'
  },
  qualifications: {
    type: [qualificationSchema],
    default: [],
    name: 'Qualifications'
  },
  documents: {
    type: [documentSchema],
    default: [],
    name: "Documents",
  },
  designation: {
    type: String, name: 'Designation'
  },
  bank: { type: bankSchema, name: "Bank Details" },
  classrooms: [{
    classroom: {
      type: Schema.Types.ObjectId,
      ref: 'Classroom',
      resolveBy: 'name',
      name: 'Classroom'
    },
    isClassTeacher: {
      type: Boolean,
      default: false,
      name: 'Is Class Teacher'
    }
  }],
  subjects: [{ type: Schema.Types.ObjectId, ref: 'Subject', resolveBy: 'name', name: 'Subjects' }],
  departments: [{ type: Schema.Types.ObjectId, ref: 'Department', resolveBy: 'name', name: 'Departments' }],
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
  signature: {
    type: String,
    name: 'Signature URL'
  },
  userId: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
    name: 'User ID'
  },
  isDeleted: {
    type: Boolean,
    default: false,
    name: 'Is Deleted',
  },
  username: {
    type: String,
    name: "Username",
    trim: true,
  },
  password: {
    type: String,
    name: "Password",
    trim: true,
  },
  user: {
    type: Schema.Types.ObjectId,
    ref: 'User', resolveBy: "username",
    required: true,
    name: 'User'
  }
}, { timestamps: true });
module.exports = mongoose.model('Teacher', teacherSchema);

```
