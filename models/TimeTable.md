```javascript
// File: ./features/time-table/time-table.model
const mongoose = require("mongoose");
const { Schema } = mongoose;
// Define days of week enum
const DaysOfWeek = Object.freeze({
  MONDAY: "Monday",
  TUESDAY: "Tuesday",
  WEDNESDAY: "Wednesday",
  THURSDAY: "Thursday",
  FRIDAY: "Friday",
  SATURDAY: "Saturday",
  SUNDAY: "Sunday",
});
const TimetableSlotSchema = new Schema({
  day: {
    type: String,
    enum: Object.values(DaysOfWeek),
    required: true,
  },
  startTime: {
    type: Number, // HH:mm format // max : 1440 (24:00)
    required: true,
    validate: {
      validator: (v) => v >= 0 && v < 1440,
      message: (props) =>
        `${props.value} is not a valid time (expected 0-1440 minutes)`,
    },
  },
  endTime: {
    type: Number, // HH:mm format
    required: true,
    validate: {
      validator: function (v) {
        return v > this.startTime && v <= 1440; // endTime must be after startTime
      },
      message: (props) =>
        `endTime (${props.value}) must be after startTime (${this.startTime})`,
    },
  },
  // course : {
  //     type: Schema.Types.ObjectId,
  //     ref: 'Course',
  //     required: true
  // },
  subject: {
    type: Schema.Types.ObjectId,
    ref: "Subject", resolveBy: "name",
    required: true,
  },
  teacher: {
    type: Schema.Types.ObjectId,
    ref: "User", resolveBy: "username",
    required: true,
  },
});
// Timetable for a specific classroom
const TimetableSchema = new Schema(
  {
    name: {
      type: String,
      required: true, // e.g. "Spring 2025 - Batch A"
    },
    description: {
      type: String,
      default: "", // optional description
    },
    classroom: {
      type: Schema.Types.ObjectId,
      ref: "Classroom", // classroom this timetable applies to
      required: true,
    },
    slots: {
      type: [TimetableSlotSchema],
      // validate: [
      //   {
      //     validator: validateNoConflicts,
      //     message: 'Slots conflict detected: overlapping times for the same instructor or classroom on the same day.'
      //   },
      //   {
      //     validator: asyncSlotsValidator,
      //     isAsync: true,
      //     message: 'Async validation failed: potential course conflict in other timetables.'
      //   }
      // ]
    },
    settings: {
      workingDays: {
        type: [String],
        enum: Object.values(DaysOfWeek),
        default: [
          DaysOfWeek.MONDAY,
          DaysOfWeek.TUESDAY,
          DaysOfWeek.WEDNESDAY,
          DaysOfWeek.THURSDAY,
          DaysOfWeek.FRIDAY,
        ],
      },
      dayStart: {
        type: Number,
        default: 480, // 8:00 AM
      },
      dayEnd: {
        type: Number,
        default: 1020, // 18:00 PM
      },
      slotDuration: {
        type: Number,
        default: 45, // 45 minutes
      },
      breakDuration: {
        type: Number,
        default: 15, // 15 minutes
      },
    },
    status: {
      type: String,
      enum: ["active", "archived", "draft"],
      default: "draft", // status of the timetable
    },
    user: {
      type: Schema.Types.ObjectId,
      ref: "User", resolveBy: "username", // user who created this timetable
      required: true,
    },
    isDeleted: {
      type: Boolean,
      default: false, // soft delete flag
    },
  },
  { timestamps: true }
);
TimetableSchema.statics.getCollisions = async function (timetableId) {
  const mongoose = require("mongoose");
  const ObjectId = mongoose.Types.ObjectId;
  return this.aggregate([
    // 1. Match the current timetable
    {
      $match: {
        _id: new ObjectId(timetableId),
        isDeleted: false,
        status: "active",
      },
    },
    // 2. Project minimal fields and sanitize slots
    {
      $project: {
        _id: 1,
        classroom: 1,
        // settings: 1,
        slots: {
          $filter: {
            input: { $ifNull: ["$slots", []] },
            as: "s",
            cond: {
              $and: [
                { $ne: ["$$s", null] },
                { $ne: ["$$s.day", null] },
                { $ne: ["$$s.startTime", null] },
                { $ne: ["$$s.endTime", null] },
                { $gt: ["$$s.endTime", "$$s.startTime"] },
              ],
            },
          },
        },
      },
    },
    // 3. Unwind slots
    { $unwind: { path: "$slots", preserveNullAndEmptyArrays: false } },
    // 4. Flatten for easier reference
    {
      $project: {
        timetableId: "$_id",
        classroom: 1, // Current timetable classroom
        day: "$slots.day",
        startTime: "$slots.startTime",
        endTime: "$slots.endTime",
        teacher: "$slots.teacher",
        subject: "$slots.subject",
      },
    },
    // 5. Lookup TEACHER collisions
    // Find other slots where the SAME teacher is booked at overlapping times (any classroom)
    {
      $lookup: {
        from: "timetables",
        let: {
          currentDay: "$day",
          currentStart: "$startTime",
          currentEnd: "$endTime",
          currentTeacher: "$teacher",
          currentId: "$timetableId",
        },
        pipeline: [
          {
            $match: {
              $expr: {
                $and: [
                  { $eq: ["$isDeleted", false] },
                  { $eq: ["$status", "active"] },
                  { $ne: ["$_id", "$$currentId"] }, // Exclude self
                ],
              },
            },
          },
          { $unwind: "$slots" },
          {
            $match: {
              $expr: {
                $and: [
                  { $eq: ["$slots.day", "$$currentDay"] }, // Same day
                  { $eq: ["$slots.teacher", "$$currentTeacher"] }, // Same teacher
                  { $lt: ["$slots.startTime", "$$currentEnd"] }, // Overlaps
                  { $gt: ["$slots.endTime", "$$currentStart"] },
                ],
              },
            },
          },
          {
            $project: {
              _id: 0,
              timetableId: "$_id",
              classroom: 1,
              startTime: "$slots.startTime",
              endTime: "$slots.endTime",
              type: { $literal: "teacher" },
            },
          },
        ],
        as: "teacherCollisions",
      },
    },
    // 6. Lookup CLASSROOM collisions
    // Find other slots where the SAME classroom is booked at overlapping times
    {
      $lookup: {
        from: "timetables",
        let: {
          currentDay: "$day",
          currentStart: "$startTime",
          currentEnd: "$endTime",
          currentClassroom: "$classroom",
          currentId: "$timetableId",
        },
        pipeline: [
          {
            $match: {
              $expr: {
                $and: [
                  { $eq: ["$isDeleted", false] },
                  { $eq: ["$status", "active"] },
                  { $ne: ["$_id", "$$currentId"] },
                  { $eq: ["$classroom", "$$currentClassroom"] }, // Same classroom
                ],
              },
            },
          },
          { $unwind: "$slots" },
          {
            $match: {
              $expr: {
                $and: [
                  { $eq: ["$slots.day", "$$currentDay"] },
                  { $lt: ["$slots.startTime", "$$currentEnd"] },
                  { $gt: ["$slots.endTime", "$$currentStart"] },
                ],
              },
            },
          },
          {
            $project: {
              _id: 0,
              timetableId: "$_id",
              classroom: 1,
              startTime: "$slots.startTime",
              endTime: "$slots.endTime",
              type: { $literal: "classroom" },
            },
          },
        ],
        as: "classroomCollisions",
      },
    },
    // 7. Merge collisions
    {
      $project: {
        day: 1,
        startTime: 1,
        endTime: 1,
        classroom: 1,
        timetableId: 1,
        collisions: {
          $concatArrays: ["$teacherCollisions", "$classroomCollisions"],
        },
      },
    },
    // 8. Unwind collisions to process one by one
    { $unwind: "$collisions" },
    // 9. Format output
    {
      $project: {
        _id: 0,
        collisionType: "$collisions.type",
        day: 1,
        overlapStart: { $max: ["$startTime", "$collisions.startTime"] },
        overlapEnd: { $min: ["$endTime", "$collisions.endTime"] },
        left: {
          timetableId: "$timetableId",
          classroom: "$classroom",
          startTime: "$startTime",
          endTime: "$endTime",
        },
        right: {
          timetableId: "$collisions.timetableId",
          classroom: "$collisions.classroom",
          startTime: "$collisions.startTime",
          endTime: "$collisions.endTime",
        },
      },
    },
    // 10. Calculate overlap minutes and finalize
    {
      $addFields: {
        overlapMinutes: { $subtract: ["$overlapEnd", "$overlapStart"] },
      },
    },
    // 11. Sort
    { $sort: { day: 1, overlapStart: 1 } }
  ]);
};
module.exports = mongoose.model("Timetable", TimetableSchema);

```
