# Teacher Management API Project

This project is an API-based web application designed to manage and interact with teacher data in a school system. It provides functionality for retrieving, updating, and deleting teacher information, along with managing courses they teach. The project utilizes ASP.NET Core MVC and a MySQL database.

## Features

- **Teacher Management**: Retrieve, add, update, and delete teacher details such as name, hire date, salary, and courses.
- **Course Management**: Associate teachers with courses they are teaching and manage course details.
- **Search and Filter**: Filter teachers based on hire date range and search for specific teacher details.
- **Database Interaction**: The application interacts with a MySQL database using Entity Framework Core and raw SQL queries.

## Controllers

### TeacherPageController
- **GET /api/teacherpage/info**: Returns MySQL version.
- **GET /api/teacherpage/list**: Retrieves a list of all teachers or filters them by hire date.
- **POST /api/teacherpage/list**: Filters teachers by hire date range.
- **GET /api/teacherpage/show**: Displays information of all teachers or a specific teacher by ID.
- **GET /api/teacherpage/add**: Displays a form to add a new teacher.
- **GET /api/teacherpage/delete**: Displays a form to confirm the deletion of a teacher.
  
### TeacherApiController
- **GET /api/teacherapi**: Retrieves a list of all teachers.
- **GET /api/teacherapi/{id}**: Retrieves teacher details by ID.
- **POST /api/teacherapi**: Adds a new teacher.
- **PUT /api/teacherapi/{id}**: Updates teacher details.
- **DELETE /api/teacherapi/{id}**: Deletes a teacher.

## Models

- **Teacher**: Contains properties like `Id`, `FirstName`, `LastName`, `HireDate`, `Salary`, and `Courses` (linked with courses taught by the teacher).
- **Course**: Represents a course with properties like `CourseId`, `CourseName`, `TeacherId`, and `StartDate`.

## Database

- **SchoolDbContext**: A context class used to interact with the MySQL database.
- **Teacher Data**: The project retrieves teacher information from the `teachers` table.
- **Course Data**: Courses are managed in the `courses` table, associated with teachers through the `TeacherId`.

## Technologies Used

- **ASP.NET Core MVC**: For building the API and handling HTTP requests.
- **MySQL**: For storing and retrieving teacher and course data.
- **Entity Framework Core**: For database interaction and management.
- **JSON Serialization**: For converting data to and from JSON format for API responses.

# TeacherPageController API

The `TeacherPageController` handles teacher-related API requests in an ASP.NET Core application. It interacts with a MySQL database via `SchoolDbContext` to manage teacher and course data. This controller provides endpoints for listing teachers, filtering by hire date, showing specific teacher details, and adding or deleting teachers.

## Endpoints

- **GET /api/teacherpage/info**: Retrieves the MySQL version from the database.
- **GET /api/teacherpage/list**: Lists all teachers in JSON format.
- **POST /api/teacherpage/list**: Filters teachers by hire date range.
- **GET /api/teacherpage/show**: Displays all teacher information.
- **POST /api/teacherpage/show**: Displays info for a specific teacher by ID.
- **GET /api/teacherpage/add**: Displays the form for adding a new teacher.
- **GET /api/teacherpage/delete**: Displays a confirmation form for deleting a teacher.

## Database Interaction

The controller retrieves teacher and course data from the database using SQL queries. Teachers are linked to courses by `TeacherId`.

## Models

- **Teacher**: Stores teacher details and their courses.
- **Course**: Stores course details and the associated teacher.

## Views

- **List.cshtml**: Displays a list of teachers.
- **Show.cshtml**: Displays detailed teacher info.
- **New.cshtml**: Displays the form to add a teacher.
- **DeleteConfirm.cshtml**: Displays a teacher deletion confirmation form.

The controller ensures efficient data retrieval and presents teacher information in JSON format, making it suitable for frontend integration.

# TeacherApiController

The `TeacherApiController` handles teacher-related API requests for retrieving, adding, updating, and deleting teacher information via HTTP requests. It utilizes `SchoolDbContext` to interact with the database.

## Endpoints

- **GET /api/teacherapi**: Retrieves a list of all teachers.
- **GET /api/teacherapi/{id}**: Retrieves a specific teacher by their ID.
- **POST /api/teacherapi**: Adds a new teacher to the database.
- **PUT /api/teacherapi/{id}**: Updates an existing teacher's information.
- **DELETE /api/teacherapi/{id}**: Deletes a teacher from the database.

## Database Interaction

- **Teacher**: The API interacts with the `Teacher` model, storing details such as name, hire date, salary, and courses taught.
- **CRUD Operations**: Basic CRUD operations are implemented to manage teacher data in the database.

## Models

- **Teacher**: Represents a teacher entity with properties such as `Id`, `FirstName`, `LastName`, `HireDate`, `Salary`, and a list of associated `Courses`.

## Example Requests

- **GET /api/teacherapi**: Returns all teachers.
- **GET /api/teacherapi/{id}**: Returns teacher details for the specified ID.
- **POST /api/teacherapi**: Adds a new teacher with a JSON payload.
- **PUT /api/teacherapi/{id}**: Updates teacher information by ID.
- **DELETE /api/teacherapi/{id}**: Deletes the specified teacher by ID.

