```javascript
// File: ./features/uploaded-document/uploaded-document.model
const mongoose = require("mongoose");
const uploadedDocumentsSchema = new mongoose.Schema({
    name: {
        type: String,
        required: true
    },
    url: {
        type: String,
        required: true
    },
    type: {
        type: String,
        required: true
    },
    size: {
        type: Number,
        required: true
    },
    createdBy: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true
    },
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: "User", resolveBy: "username",
        required: true
    },
    isDeleted: {
        type: Boolean,
        default: false
    }
}, { timestamps: true });
module.exports = mongoose.model("UploadedDocuments", uploadedDocumentsSchema);
```
