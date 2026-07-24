```javascript
// File: ./features/employee/employee.model
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
// Experience sub-schema
const experienceSchema = new Schema({
    organization: {
        type: String,
        name: 'Organization'
    },
    position: {
        type: String,
        name: 'Position'
    },
    years: {
        type: Number,
        name: 'Years of Experience'
    }
}, { _id: false });
// Qualification sub-schema
const qualificationSchema = new Schema({
    qualification: {
        type: String, required: true,
        name: 'Qualification'
    },
    college: {
        type: String,
        name: 'College'
    },
    passingYear: {
        type: Number,
        name: 'Passing Year'
    }
}, { _id: false });
// Email validator
function validateEmail(email) {
    return /.+@.+\..+/.test(email);
}
// Employee schema
const employeeSchema = new Schema({
    joiningDate: {
        type: Date,
        name: 'Joining Date',
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
    // Personal Details
    firstName: {
        type: String,
        required: true,
        name: 'First Name',
    },
    lastName: {
        type: String,
        name: 'Last Name'
    },
    phone: {
        type: String,
        required: true,
        name: 'Mobile Number',
    },
    email: {
        type: String,
        required: true,
        unique: true,
        validate: [validateEmail, 'Invalid email'],
        name: 'Email',
    },
    dateOfBirth: {
        type: Date,
        name: 'Date of Birth'
    },
    gender: {
        type: String,
        enum: Object.values(GenderEnum),
        name: 'Gender'
    },
    fatherName: {
        type: String,
        name: "Father's Name"
    },
    spouseName: {
        type: String,
        name: "Spouse's Name"
    },
    maritalStatus: {
        type: String,
        name: 'Marital Status'
    },
    nationality: {
        type: String, default: 'INDIAN',
        name: 'Nationality'
    },
    religion: {
        type: String,
        name: 'Religion'
    },
    category: {
        type: String,
        name: 'Category'
    },
    role: {
        type: String, required: true,
        name: 'Role',
    },
    designation: {
        type: String,
        name: 'Designation'
    },
    experiences: {
        type: [experienceSchema],
        default: [],
        name: 'Experiences'
    },
    qualifications: {
        type: [qualificationSchema],
        default: [],
        name: 'Qualifications'
    },
    // Contact Address
    address: {
        type: addressSchema,
        name: 'Address'
    },
    bank: { type: bankSchema, name: "Bank Details" },
    documents: {
        type: [{
            label: String,
            value: String,
            identifier: String
        }],
        default: [],
        name: 'Documents'
    },
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
    // Authentication
    username: {
        type: String,
        required: true,
        unique: true,
        name: 'Username',
    },
    password: {
        type: String,
        required: true,
        name: 'Password'
    },
    userId: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        name: 'User ID'
    },
    // Soft Delete
    isDeleted: {
        type: Boolean, default: false,
        name: 'Is Deleted',
    },
    avatar: {
        type: String,
        name: 'Avatar'
    },
    user: {
        type: Schema.Types.ObjectId,
        ref: 'User', resolveBy: "username",
        name: 'User'
    }
}, { timestamps: true });
// Attach enums to statics
employeeSchema.statics.GenderEnum = GenderEnum;
module.exports = mongoose.model('Employee', employeeSchema);
```
