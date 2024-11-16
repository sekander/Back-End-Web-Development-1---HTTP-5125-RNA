# Teacher App API

This project provides an API for managing teacher and course data from a school database. It allows retrieving information about teachers, their courses, and filtering teachers by their hire date range. The data is returned in JSON format and is used to display teacher and course details on the front-end.

## Features

- **Get Teacher Info**: Retrieve all teachers' information.
- **Get Teacher by Hire Date Range**: Filter teachers by hire date.
- **Get Specific Teacher Info**: Retrieve information about a specific teacher by their ID.
- **Get MySQL Version**: Retrieve the MySQL version used by the database.

## Code Overview

### File Structure

- **TeacherApiController.cs**: Contains the API controller that handles HTTP requests related to teacher and course data.
- **SchoolDbContext.cs**: Contains the database context to interact with the school database.

### `TeacherApiController.cs`

This is the main controller responsible for handling requests related to teacher and course information. It interacts with a database to retrieve and manage teacher and course data. It also handles filtering of teachers based on hire date and serves the data as JSON to the front-end.

#### Constructor (`TeacherApiController(SchoolDbContext db)`)

- The constructor initializes the controller with an instance of `SchoolDbContext`, which is used to query the database.
- The constructor retrieves data from the `teachers` and `courses` tables and assigns them to the `_teacherList` and `_coursesList` respectively.
- It then associates each course with its corresponding teacher using the `teacherid` field.

#### Methods

1. **Info (GET `/api/teacherpage/info`)**
   - Retrieves the version of MySQL from the database and returns it as a response.
   - Uses the `_db.GetMySqlVersion()` method to get the MySQL version and returns it in the response.

2. **TeacherInfo (GET `/api/teacherpage/list`)**
   - Retrieves and returns all teacher information as a JSON response.
   - It serializes the list of teachers (`_teacherList`) to JSON format using `JsonSerializer.Serialize()` with indented formatting for readability.
   - Passes the serialized teacher data to the view.

3. **TeacherInfo (POST `/api/teacherpage/list`)**
   - Filters teachers by their hire date range (provided via `startRange` and `endRange` form data).
   - If the hire date range is valid, it filters the teachers who were hired within the specified range and returns the filtered list in JSON format.
   - If no teachers match the criteria, a message saying "Teacher not found" is returned.

4. **AllTeacherInfo (GET `/api/teacherpage/show`)**
   - Displays all teachers' information.
   - This view renders a list of all teachers without any filtering.

5. **AllTeacherInfo (POST `/api/teacherpage/show`)**
   - Retrieves and displays information about a specific teacher based on their `teacherId` (provided via form data).
   - If the teacher ID is valid, it searches for the corresponding teacher in `_teacherList` and returns the teacher's information as a JSON response.
   - If the teacher ID is invalid or not found, it returns an error message.

### Data Flow and Database Interaction

1. **Database Querying**
   - The database is queried using raw SQL queries via the `_db.ExecuteQuery(query)` method.
   - The data is fetched from the `teachers` and `courses` tables, and the results are processed into `Teacher` and `Course` objects.
   - After fetching the data, each teacher is associated with their respective courses by matching `teacherid` fields between the `teachers` and `courses` tables.

2. **Filtering Teachers by Hire Date**
   - Teachers can be filtered by their hire date using the `TeacherInfo` method. The dates are provided via form data, parsed into `DateTime` objects, and then used to filter the list of teachers by comparing their `HireDate` property.

3. **JSON Serialization**
   - The data (teacher and course information) is serialized to JSON using the `JsonSerializer.Serialize()` method.
   - The data is returned to the front-end in an easy-to-read, indented JSON format, making it suitable for rendering or further processing on the client-side.

### Models

1. **Teacher**
   - Contains properties like `Id`, `FirstName`, `LastName`, `EmployeeNumber`, `HireDate`, and `Salary`.
   - The `Courses` property holds the list of courses assigned to the teacher.

2. **Course**
   - Contains properties like `CourseId`, `TeacherId`, `CourseCode`, `CourseName`, `StartDate`, and `EndDate`.
   - The `TeacherId` property is used to associate a course with a specific teacher.

### Example Requests

#### 1. Get All Teacher Info

**GET** `/api/teacherpage/list`

**Response:**
```json
[
  {
    "Id": 1,
    "FirstName": "John",
    "LastName": "Doe",
    "EmployeeNumber": "T12345",
    "HireDate": "2015-06-15T00:00:00",
    "Salary": 50000,
    "Courses": [
      {
        "CourseId": 101,
        "CourseCode": "CS101",
        "CourseName": "Introduction to Computer Science",
        "StartDate": "2022-09-01",
        "EndDate": "2022-12-15"
      }
    ]
  }
]


