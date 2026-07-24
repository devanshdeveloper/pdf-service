```javascript
// File: ./features/book/book.model
const mongoose = require('mongoose');
const Schema = mongoose.Schema;
// Availability enum for book copies
const BookAvailability = Object.freeze({
    AVAILABLE: 'available',
    CHECKED_OUT: 'checked_out',
    LOST: 'lost',
    RESERVED: 'reserved'
});
const bookSchema = new Schema({
    title: {
        type: String,
        required: true,
        trim: true,
    },
    image: {
        type: String,
        trim: true,
    },
    author: {
        type: String,
        required: true,
        trim: true,
    },
    isbn: {
        type: String,
        required: true,
        unique: true,
        trim: true,
        validate: {
            // simple ISBN-10 or ISBN-13 pattern
            validator: v => /^(?:\d{9}X|\d{10}|\d{13})$/.test(v),
            message: props => `${props.value} is not a valid ISBN`
        }
    },
    publishedDate: {
        type: Date,
    },
    copies: [{
        transaction: {
            type: Schema.Types.ObjectId,
            ref: 'BorrowingTransaction',
        },
        barcode: {
            type: String,
            unique: true,
            trim: true
        }, // or auto-generated
        location: {
            type: String,
            trim: true
        }, // branch/shelf
        state: {
            type: String,
            enum: Object.values(BookAvailability),
            default: BookAvailability.AVAILABLE,
        },
    }],
    availability: {
        type: String,
        enum: Object.values(BookAvailability),
        default: BookAvailability.AVAILABLE,
    },
    course: {
        type: Schema.Types.ObjectId,
        ref: 'Course', resolveBy: 'title'
    },
    department: {
        type: Schema.Types.ObjectId,
        ref: 'Department', resolveBy: "name",
    },
    stream: {
        type: Schema.Types.ObjectId,
        ref: 'Stream', resolveBy: "name"
    },
    subject: {
        type: Schema.Types.ObjectId,
        ref: 'Subject', resolveBy: "name"
    },
    isDeleted: {
        type: Boolean,
        default: false,
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true
    }
}, {
    timestamps: true
});
module.exports = mongoose.model('Book', bookSchema);
```
