```javascript
// File: ./features/user/UserHelper
const { ModelHelper } = require("../../helpers/ModelHelper");
const { safeFormat } = require("../../helpers/FunctionHelper");
const User = require("../auth/User.model");
const AuthStateHelper = require("../../helpers/AuthStateHelper");
const AuthHelper = require("../../helpers/AuthHelper");
const { ApplicationConstants, UserTypes } = require("../../constants");
const Subscription = require("../subscription/subscription.model");
const Handlebars = require("handlebars");
const AggregationBuilder = require("../../helpers/aggregation/AggregationBuilder");
const { Types } = require("mongoose");
const settingsModels = {
  Student: require("../settings/student/student-setting.model"),
  Teacher: require("../settings/teacher/teacher-settings.model"),
  Parent: require("../settings/parent/parent-setting.model"),
  Employee: require("../settings/employee/employee-settings.model"),
  Party: require("../settings/party/party-settings.model"),
  Customer: require("../settings/customer/customer-settings.model"),
  [UserTypes.DistrictOperator]: require("../settings/district-operator/district-operator-setting.model"),
  Partner: require("../settings/partner/partner-setting.model"),
}
const userTypeModels = {
  Student: new (require("../student/StudentHelper"))(require("../student/student.model")),
  Teacher: require("../teacher/Teacher.model"),
  Parent: require("../parent/parent.model"),
  Employee: require("../employee/employee.model"),
  Party: require("../party/party.model"),
  Customer: require("../customer/customer.model"),
  [UserTypes.DistrictOperator]: require("../district-operator/district-operator.model"),
  Partner: require("../partner/partner.model"),
}
class UserHelper extends ModelHelper {
  constructor() {
    super(User);
    this.authStateHelper = new AuthStateHelper();
    this.authHelper = new AuthHelper();
  }
  async paginate(filter = {}, options = {}) {
    const { select = "", context = {} } = options;
    const requestedFields = select?.split?.(" ") || [];
    const computedFields = [
      "sub_users_count",
      "students_count",
      "teachers_count",
      "employees_count",
      "customers_count",
      "parents_count",
      "parties_count",
      "partners_count",
      "district_operators_count"
    ];
    const hasComputedFields = computedFields.some(field => requestedFields.includes(field));
    if (!hasComputedFields) {
      return super.paginate(filter, options);
    }
    const builder = new AggregationBuilder();
    const userTypes = context.user?.types || [];
    // Helper strict check for user type
    const hasType = (type) => userTypes.includes(type);
    builder
      .castFilter(filter)
      // 1. sub_users_count
      .check(requestedFields.includes("sub_users_count"), (b) => {
        b.lookupMatchCount({
          from: "users",
          localField: "_id",
          foreignField: "user",
          as: "sub_users_count_data"
        })
          .extractCount("sub_users_count", "sub_users_count_data")
          .project({ sub_users_count_data: 0 });
      })
      // 2. students_count (Admin)
      .check(requestedFields.includes("students_count") && hasType(UserTypes.Admin), (b) => {
        b.lookupMatchCount({
          from: "students",
          localField: "_id",
          foreignField: "user",
          as: "students_count_data"
        })
          .extractCount("students_count", "students_count_data")
          .project({ students_count_data: 0 });
      })
      // 3. teachers_count (Admin)
      .check(requestedFields.includes("teachers_count") && hasType(UserTypes.Admin), (b) => {
        b.lookupMatchCount({
          from: "teachers",
          localField: "_id",
          foreignField: "user",
          as: "teachers_count_data"
        })
          .extractCount("teachers_count", "teachers_count_data")
          .project({ teachers_count_data: 0 });
      })
      // 4. employees_count (Admin)
      .check(requestedFields.includes("employees_count") && hasType(UserTypes.Admin), (b) => {
        b.lookupMatchCount({
          from: "employees",
          localField: "_id",
          foreignField: "user",
          as: "employees_count_data"
        })
          .extractCount("employees_count", "employees_count_data")
          .project({ employees_count_data: 0 });
      })
      // 5. customers_count (Business)
      .check(requestedFields.includes("customers_count") && hasType(UserTypes.Business), (b) => {
        b.lookupMatchCount({
          from: "customers",
          localField: "_id",
          foreignField: "user",
          as: "customers_count_data"
        })
          .extractCount("customers_count", "customers_count_data")
          .project({ customers_count_data: 0 });
      })
      // 6. parents_count (Admin)
      .check(requestedFields.includes("parents_count") && hasType(UserTypes.Admin), (b) => {
        b.lookupMatchCount({
          from: "parents",
          localField: "_id",
          foreignField: "user",
          as: "parents_count_data"
        })
          .extractCount("parents_count", "parents_count_data")
          .project({ parents_count_data: 0 });
      })
      // 7. parties_count (Business)
      .check(requestedFields.includes("parties_count") && hasType(UserTypes.Business), (b) => {
        b.lookupMatchCount({
          from: "parties",
          localField: "_id",
          foreignField: "user",
          as: "parties_count_data"
        })
          .extractCount("parties_count", "parties_count_data")
          .project({ parties_count_data: 0 });
      })
      // 8. partners_count (SuperAdmin)
      .check(requestedFields.includes("partners_count") && hasType(UserTypes.SuperAdmin), (b) => {
        b.lookupMatchCount({
          from: "partners",
          localField: "_id",
          foreignField: "userId", // Note: Partner model has userId linking to User
          as: "partners_count_data"
        })
          .extractCount("partners_count", "partners_count_data")
          .project({ partners_count_data: 0 });
      })
      // 9. district_operators_count (SuperAdmin)
      .check(requestedFields.includes("district_operators_count") && hasType(UserTypes.SuperAdmin), (b) => {
        b.lookupMatchCount({
          from: "districtoperators",
          localField: "_id",
          foreignField: "userId", // Note: DistrictOperator model has userId linking to User
          as: "district_operators_count_data"
        })
          .extractCount("district_operators_count", "district_operators_count_data")
          .project({ district_operators_count_data: 0 });
      })
      // Sort
      .check(!!options.sort, (b) => b.sort(options.sort));
    const pipeline = builder.build();
    return this.paginatedAggregate(pipeline, {
      page: options.page,
      limit: options.limit
    });
  }
  async createAccount({ type = "Student", username, password, createAccount, _id, data, contextUser }) {
    const settingsModel = settingsModels[type];
    const userModel = userTypeModels[type];
    if (!settingsModel) {
      throw new Error("Invalid user type");
    }
    if (!userModel) {
      throw new Error("Invalid user type");
    }
    if (data && !_id) {
      await userModel.validate({ ...data, user: contextUser._id });
    }
    if (!data && _id) {
      data = await userModel.findById(_id).lean();
    }
    let settings;
    if (createAccount == null || createAccount == undefined || !username || !password) {
      settings = await settingsModel.findOne({
        user: contextUser?._id,
      })
      if (!settings) {
        // Fallback or throw? If no settings, we can't generate username/password from template
        // But if username/password IS provided, we might proceed.
        // If not provided and no settings, we must throw or return error.
        if (!username || !password) {
          throw new Error("Settings not found and credentials not provided");
        }
      } else {
        if (createAccount == null || createAccount == undefined) {
          createAccount = settings?.accountCreationEnabled;
        }
        if (!username) {
          username = settings?.templates?.username;
        }
        if (!password) {
          password = settings?.templates?.password;
        }
      }
    }
    let user;
    let userId = null;
    let userRecord = null;
    let parsedUsername = null;
    let parsedPassword = null;
    const templatePayload = {
      ...data,
      birthYear: safeFormat(data?.dateOfBirth, "yyyy") || "",
      birthMonth: safeFormat(data?.dateOfBirth, "MM") || "",
      birthDate: safeFormat(data?.dateOfBirth, "dd") || "",
    }
    // if (type === "Student") {
    //   templatePayload.classroomCounter = await Classroom.countDocuments({ user: contextUser._id });
    // }
    if (username && password && createAccount) {
      const usernameTemplate = Handlebars.compile(username);
      const passwordTemplate = Handlebars.compile(password);
      parsedUsername = usernameTemplate(templatePayload);
      parsedPassword = passwordTemplate(templatePayload);
      const hashedPassword = await this.authHelper.hashPassword(parsedPassword); // authHelper was also missing, assumed access via this or global? 
      // UserHelper extends ModelHelper. 
      // In line 78 of original: 'await authHelper.hashPassword' -> authHelper is likely a global or require.
      // But looking at top of file, no AuthHelper import? 
      // Wait, line 30: this.authStateHelper = new AuthStateHelper();
      // But AuthStateHelper usually handles state, not hashing?
      // Let's check where hashing comes from. 
      // In original file line 78: `await authHelper.hashPassword(parsedPassword);`
      // I don't see `authHelper` imported.
      // I see `AuthStateHelper` imported.
      // I see `const { AuthHelper } = require("../../helpers");` in parent.route.js. 
      // Maybe I should add AuthHelper import to UserHelper.js?
      const userData = {
        ...data,
        type,
        username: parsedUsername,
        password: hashedPassword,
        authState: "Complete",
        emailVerified: true,
        provider: "admin",
        record: _id,
        user: contextUser._id,
      }
      user = await User.create(userData)
      userId = user._id
      if (_id) {
        userRecord = await userModel.findOneAndUpdate({ _id: _id }, { userId: userId })
      } else if (data && !_id) {
        userRecord = await userModel.create({ ...data, userId: userId, user: contextUser._id })
      }
    }
    if (!userRecord) {
      userRecord = await userModel.create({
        ...data, userId: userId, user: contextUser._id,
        username: parsedUsername || "",
        password: parsedPassword || "",
      })
    }
    return { user, settings, userId, userRecord }
  }
  // Add user-specific methods here
  async findByEmail(email) {
    return this.findOne({ email });
  }
  async updatePassword(id, password) {
    return this.update(id, { password });
  }
  async verifyEmail(id) {
    return this.update(id, { emailVerified: true });
  }
  async updateProfile(id, data) {
    const allowedFields = ["name", "address", "phone", "avatar"];
    const updateData = {};
    Object.keys(data).forEach((key) => {
      if (allowedFields.includes(key)) {
        updateData[key] = data[key];
      }
    });
    return this.update(id, updateData);
  }
  async Response(user) {
    if (!user) {
      return null;
    }
    if (typeof user === "string") {
      user = await User.findById(user);
    }
    let subscription = null;
    let permissions = null;
    const isSubUserType = ApplicationConstants.isSubUserType(user?.type);
    if (isSubUserType) {
      if (user?.user) {
        subscription = await Subscription.findOne({
          status: "active",
          user: user.user
        });
        user.subscription = subscription;
      }
      permissions = ApplicationConstants.getPermissionsByUserType(user?.type, null, { defaultPermission: ["CRUD", -1, -1] });
    } else {
      permissions = ApplicationConstants.getPermissionsByUserType(user?.type, null, { defaultPermission: ["CRUD", -1, -1] });
      subscription = await Subscription.findOne({
        status: "active",
        user: user._id
      });
      user.subscription = subscription;
    }
    let courses = null;
    let classrooms = null;
    let departments = null;
    let subjects = null;
    let students = null;
    let teacher = null;
    let parent = null;
    let student = null;
    let districtOperator = null;
    let partner = null;
    if (user.type === UserTypes.Teacher) {
      teacher = await this.resolveUserRecord(user);
      if (teacher) {
        courses = teacher.courses
        classrooms = teacher.classrooms
        departments = teacher.departments
        subjects = teacher.subjects
      }
    }
    if (user.type === UserTypes.Student) {
      student = await this.resolveUserRecord(user);
      if (student) {
        // courses = student.courses
        // classrooms = student.classrooms
        // departments = student.departments
        subjects = student.subjects
      }
    }
    if (user.type === UserTypes.Parent) {
      parent = await this.resolveUserRecord(user);
      if (parent) {
        students = parent.students
        // courses = parent.courses
        // classrooms = parent.classrooms
        // departments = parent.departments
        // subjects = parent.subjects
      }
    }
    if (user.type === UserTypes.DistrictOperator) {
      districtOperator = await this.resolveUserRecord(user);
      if (districtOperator) {
      }
    }
    if (user.type === UserTypes.Partner) {
      partner = await this.resolveUserRecord(user);
      if (partner) {
      }
    }
    return {
      _id: user._id,
      name: user.name,
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      type: user.type,
      types: user.types,
      avatarUrl: user.avatarUrl,
      permissions,
      subscription: user.subscription,
      courses,
      classrooms,
      departments,
      subjects,
      students,
      teacher,
      parent,
      student,
      districtOperator,
      partner,
    };
  }
  async getCurrentAuthState(userId) {
    const user = await this.findById(userId);
    return user ? user.authState : null;
  }
  async updateAuthState(userId, newState) {
    if (!this.authStateHelper.isValidState(newState)) {
      throw new Error("Invalid authentication state");
    }
    const currentState = await this.getCurrentAuthState(userId);
    if (!this.authStateHelper.isValidTransition(currentState, newState)) {
      throw new Error("Invalid state transition");
    }
    return await this.update(userId, { authState: newState });
  }
  async progressToNextState(userId) {
    const currentState = await this.getCurrentAuthState(userId);
    const nextState = this.authStateHelper.getNextState(currentState);
    if (!nextState) {
      throw new Error("Already in final state");
    }
    return await this.updateAuthState(userId, nextState);
  }
  async validateStateRequirements(userId, requiredStates) {
    const currentState = await this.getCurrentAuthState(userId);
    return this.authStateHelper.areRequiredStatesCompleted(
      currentState,
      requiredStates
    );
  }
  async resetAuthState(userId) {
    const initialState = this.authStateHelper.getInitialState();
    return await this.update(userId, { authState: initialState });
  }
  async isInFinalState(userId) {
    const currentState = await this.getCurrentAuthState(userId);
    return this.authStateHelper.isFinalState(currentState);
  }
  async resolveUserRecord(userId, contextUser) {
    if (!userId) {
      throw new Error("Invalid user ID");
    }
    let user = null;
    if (typeof userId == "string") {
      user = await this.findById(userId)
    } else {
      user = userId;
    }
    const type = user.type;
    const userModel = userTypeModels[type];
    if (!userModel) {
      throw new Error("Invalid user type");
    }
    const filter = { userId: user._id };
    if (contextUser?._id) {
      filter.user = contextUser._id;
    }
    let userRecord = await userModel.findOne(filter);
    if (!userRecord) {
      const createData = {
        ...user,
        userId: user._id,
      }
      if (contextUser?._id) {
        createData.user = contextUser._id;
      }
      userRecord = await userModel.create({ ...user, userId: user._id });
    }
    await this.update(userId, { record: userRecord._id });
    return userRecord;
  }
  async resolverUsersByParentUser(userId) {
    const ids = await this.find({ user: userId });
    return await Promise.all(ids.map(id => this.resolveUserRecord(id)));
  }
}
module.exports = UserHelper;

```
