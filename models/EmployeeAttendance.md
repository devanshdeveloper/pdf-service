```javascript
// File: ./features/employee-attendance/employee-attendance.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
// ========== Attendance Model ==========
// AttendanceStatus enum
const AttendanceStatus = Object.freeze({
  PRESENT: "present",
  ABSENT: "absent",
  LATE: "late",
  EXCUSED: "excused",
});
const attendanceSchema = new Schema(
  {
    employee: {
      type: Schema.Types.ObjectId,
      ref: "Employee",
      required: true,
    },
    start: {
      type: Date,
      required: true,
    },
    end: {
      type: Date,
      required: true,
    },
    allDay: {
      type: Boolean,
      default: false,
    },
    status: {
      type: String,
      required: true,
      enum: Object.values(AttendanceStatus),
      default: AttendanceStatus.PRESENT,
    },
    remark: {
      type: String,
      trim: true,
    },
    recordedBy: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username",
      required: true,
    },
    isDeleted: {
      type: Boolean,
      default: false,
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
attendanceSchema.statics.AttendanceStatus = AttendanceStatus;
module.exports = mongoose.model("EmployeeAttendance", attendanceSchema);

```
