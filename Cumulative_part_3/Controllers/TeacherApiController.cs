using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using TeacherApp.Models;

namespace TeacherApp.Controllers
{
    /// <summary>
    /// Controller for handling API requests related to teachers and courses in the school database.
    /// This API provides endpoints for fetching information about teachers, courses, students, and individual teacher details.
    /// </summary>
    [Route("api/[controller]")]
    public class TeacherApiController : Controller
    {
        private readonly SchoolDbContext _db;
        // List to store teachers
        private static readonly List<Teacher> _teacherList = [];
        // List to store courses
        private static readonly List<Course> _coursesList = [];
        // Static options for JSON serialization with indentation.
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
        //Makes sure if update db is only executed once in the controller constructor
        private static bool _hasExecuted;


        /// <summary>
        /// Initializes a new instance of the class.
        /// This constructor loads teachers and courses data from the database and associates courses with the respective teachers.
        /// </summary>
        /// <param name="db">The database context used to execute queries against the database.</param>
        public TeacherApiController(SchoolDbContext db)
        {
            // Stores the database context for later use.
            _db = db;
            if (!_hasExecuted)
            {
                UpdateDB();
                _hasExecuted = true;
            }
        }

        private void UpdateDB()
        {
            // Query to fetch teachers' data from the database
            string query = "SELECT * FROM  teachers";
            DataTable dataTable = _db.ExecuteQuery(query);

            _teacherList.Clear();
            _coursesList.Clear();

            // If there are any rows in the teacher data table
            if (dataTable.Rows.Count > 0)
            {
                // Iterating through each row and mapping columns to teacher properties
                foreach (DataRow row in dataTable.Rows)
                {
                    Teacher _teacher = new();
                    // Loop through columns
                    foreach (DataColumn column in dataTable.Columns) // Corrected this line
                    {

                        String columnString = column.ToString();
                        // Switch based on column name to populate teacher properties
                        switch (columnString)
                        {
                            case "teacherid":
                                if (int.TryParse(row[column].ToString(), out int teacherId))
                                {
                                    _teacher.Id = teacherId;
                                }
                                break;
                            case "teacherfname":
                                _teacher.FirstName = (String)row[column];
                                break;
                            case "teacherlname":
                                _teacher.LastName = (String)row[column];
                                break;
                            case "employeenumber":
                                _teacher.EmployeeNumber = (String)row[column];
                                break;
                            case "hiredate":
                                if (DateTime.TryParse(row[column].ToString(), out DateTime hireDate))
                                {
                                    _teacher.HireDate = hireDate;
                                }
                                break;
                            case "salary":
                                if (decimal.TryParse(row[column].ToString(), out decimal salary))
                                {
                                    _teacher.Salary = salary;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    // Add teacher to the list
                    _teacherList.Add(_teacher);
                }
            }

            // Query to fetch course data from the database
            query = "SELECT * FROM  courses";
            dataTable = _db.ExecuteQuery(query);

            // If there are any rows in the course data table
            if (dataTable.Rows.Count > 0)
            {
                // Iterating through each row and mapping columns to course properties
                foreach (DataRow row in dataTable.Rows)
                {
                    Course _course = new();
                    foreach (DataColumn column in dataTable.Columns) // Corrected this line
                    {

                        String columnString = column.ToString();

                        // Switch based on column name to populate course properties
                        switch (columnString)
                        {
                            case "courseid":
                                if (int.TryParse(row[column].ToString(), out int courseId))
                                {
                                    _course.CourseId = courseId;
                                }
                                break;
                            case "teacherid":
                                if (int.TryParse(row[column].ToString(), out int teacherId))
                                {
                                    _course.TeacherId = teacherId;
                                }
                                break;
                            case "coursecode":
                                _course.CourseCode = (string)row[column];
                                break;
                            case "coursename":
                                _course.CourseName = (string)row[column];
                                break;
                            case "startdate":
                                if (DateTime.TryParse(row[column].ToString(), out DateTime startDate))
                                {
                                    _course.StartDate = startDate;
                                }
                                break;
                            case "finishdate":
                                if (DateTime.TryParse(row[column].ToString(), out DateTime endDate))
                                {
                                    _course.EndDate = endDate;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    // Add course to the list`
                    _coursesList.Add(_course);
                }
            }

            // Now, assign courses to the respective teacher based on teacherid
            foreach (Teacher teacher in _teacherList)
            {
                // Find courses for this teacher based on teacherid
                List<Course> teacherCourses = _coursesList.Where(c => c.TeacherId == teacher.Id).ToList();

                // Add the courses to the teacher
                teacher.Courses.AddRange(teacherCourses);
            }
        }


        /// <summary>
        /// Retrieves the version of the MySQL server.
        /// </summary>
        /// <returns>Returns the MySQL version as a string.</returns>
        /// <example>
        /// GET /api/teacher/info
        /// Response:
        /// "Teacher API"
        /// </example>
        [HttpGet("info")]
        public IActionResult Info()
        {
            string? mysqlVersion = _db.GetMySqlVersion();
            Console.WriteLine($"MySQL version: {mysqlVersion}");
            return Ok("Teacher API");
        }

        /// <summary>
        /// Retrieves all teachers' information.
        /// </summary>
        /// <returns>Returns a JSON array of teacher objects containing all teacher details.</returns>
        /// <example>
        /// curl "http://localhost:5074/api/teacherapi/all"
        /// GET /api/teacher/all
        /// Response:
        /// [
        ///     {
        ///         "Id": 1,
        ///         "FirstName": "John",
        ///         "LastName": "Doe",
        ///         "EmployeeNumber": "E12345",
        ///         "HireDate": "2015-08-23T00:00:00",
        ///         "Salary": 50000,
        ///         "Courses": [
        ///             {
        ///                 "CourseId": 101,
        ///                 "CourseCode": "CS101",
        ///                 "CourseName": "Introduction to Computer Science",
        ///                 "StartDate": "2023-01-15T00:00:00",
        ///                 "EndDate": "2023-05-15T00:00:00"
        ///             }
        ///         ]
        ///     }
        /// ]
        /// ...
        /// </example>
        [HttpGet("all")]
        public IActionResult AllTeachersInfo()
        {
            // Serialize to JSON
            string json = JsonSerializer.Serialize(_teacherList, _jsonOptions);

            // Print the JSON string
            Console.WriteLine(json);

            // Return the serialized JSON as the response
            return Ok(json);
        }

        /// <summary>
        /// Retrieves all courses' information.
        /// </summary>
        /// <returns>Returns a JSON array of course objects containing all course details.</returns>
        /// <example>
        /// GET /api/teacher/courses
        /// Response:
        /// [
        ///     {
        ///         "CourseId": 101,
        ///         "TeacherId": 1,
        ///         "CourseCode": "CS101",
        ///         "CourseName": "Introduction to Computer Science",
        ///         "StartDate": "2023-01-15T00:00:00",
        ///         "EndDate": "2023-05-15T00:00:00"
        ///     }
        /// ]
        /// ...
        /// </example>
        [HttpGet("courses")]
        public IActionResult AllCourses()
        {
            // Serialize to JSON
            string json = JsonSerializer.Serialize(_coursesList, _jsonOptions);

            // Print the JSON string
            Console.WriteLine(json);

            // Return the serialized JSON as the response
            return Ok(json);
        }

        /// <summary>
        /// Retrieves all students' information.
        /// </summary>
        /// <returns>Returns a JSON array of student objects containing all student details.</returns>
        /// <example>
        /// curl "http://localhost:5074/api/teacherapi/all"
        /// GET /api/teacher/students
        /// Response:
        /// [
        ///     {
        ///         "studentid": 1,
        ///         "firstname": "Alice",
        ///         "lastname": "Smith",
        ///         "birthdate": "2005-05-10T00:00:00",
        ///         "email": "alice.smith@example.com"
        ///     }
        /// ]
        /// ...
        /// </example>
        [HttpGet("students")]
        public IActionResult AllStudents()
        {
            List<Dictionary<string, object>> _students = [];

            string query = "SELECT * FROM  students";
            DataTable dataTable = _db.ExecuteQuery(query);

            // If there are any rows in the students data table
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Dictionary<string, object> student = [];
                    foreach (DataColumn column in dataTable.Columns) // Corrected this line
                    {
                        // Add column name and value to the dictionary
                        student.Add(column.ColumnName, row[column]);
                    }

                    // Add the student dictionary to the list
                    _students.Add(student);
                }
            }

            // Serialize to JSON
            string json = JsonSerializer.Serialize(_students, _jsonOptions);

            // Print the JSON string
            Console.WriteLine(json);

            // Return the serialized JSON as the response
            return Ok(json);
        }

        /// <summary>
        /// Retrieves the details of a specific teacher by their teacher ID.
        /// </summary>
        /// <param name="id">The teacher ID for which details are requested.</param>
        /// <returns>Returns a JSON object with the teacher's details or a NotFound response if the teacher is not found.</returns>
        /// <example>
        /// curl -X GET "http://localhost:5074/api/teacher/show?teacherId=3"
        /// GET /api/teacher/teacherinfo/1
        /// Response:
        /// {
        ///     "Id": 1,
        ///     "FirstName": "John",
        ///     "LastName": "Doe",
        ///     "EmployeeNumber": "E12345",
        ///     "HireDate": "2015-08-23T00:00:00",
        ///     "Salary": 50000,
        ///     "Courses": [
        ///         {
        ///             "CourseId": 101,
        ///             "CourseCode": "CS101",
        ///             "CourseName": "Introduction to Computer Science",
        ///             "StartDate": "2023-01-15T00:00:00",
        ///             "EndDate": "2023-05-15T00:00:00"
        ///         }
        ///     ]
        /// }
        /// </example>
        [HttpGet("teacher/{teacherID}")]
        public IActionResult TeacherInfo([FromRoute] int @teacherID)
        {
            Teacher? result = null;
            //Loop through teachers list and check for matching ID
            foreach (Teacher t in _teacherList)
            {
                if (t.Id == @teacherID)
                {
                    result = t;
                    break; // Exit the loop once the teacher is found
                }
            }

            // Serialize the found teacher to JSON
            string json = JsonSerializer.Serialize(result, _jsonOptions);

            // Print the JSON string (for debugging purposes)
            Console.WriteLine(json);

            if (json.Equals("null"))
            {
                // If teacher is not found, return NotFound response
                return NotFound("Teacher not found");
            }
            else
            {
                // Return the serialized JSON as the response
                return Ok(json);
            }
        }


        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="employeeNumber">The employee number of the teacher, should start with 'T' followed by digits.</param>
        /// <param name="firstName">The first name of the teacher.</param>
        /// <param name="lastName">The last name of the teacher.</param>
        /// <param name="hireDate">The hire date of the teacher.</param>
        /// <param name="salary">The salary of the teacher.</param>
        /// <returns>A success or failure message depending on the input data and validation checks.</returns>
        /// <example>
        /// curl -X POST "http://192.168.2.18:5000/api/teacherapi/teacheradd?employeeNumber=T1234&firstName=&lastName=Sekander&hireDate=2023-05-15&salary=55000"; 
        /// POST /api/teacher/teacheradd?employeeNumber=T12345&firstName=John&lastName=Doe&hireDate=2023-05-15&salary=55000
        /// Response: 
        /// {
        ///     "success": true,
        ///     "message": "New Teacher John Doe is added successfully"
        /// }
        /// </example>
        [HttpPost("teacheradd")]
        public IActionResult TeacherAdd(
                                      [FromQuery] string employeeNumber,
                                      [FromQuery] string firstName,
                                      [FromQuery] string lastName,
                                      [FromQuery] DateTime hireDate,
                                      [FromQuery] decimal salary
        )
        {

            DateTime currentDate = DateTime.Now;
            bool employeeNumberIsUnique = false;
            //Constructing the SQL query by directly embedding values
            string query = $@"
                INSERT INTO teachers (employeenumber, teacherfname, teacherlname, hiredate, salary)
                VALUES ('{employeeNumber}', '{firstName}', '{lastName}', '{hireDate:yyyy-MM-dd}', {salary})";

            /*
                Error Handling
                - Employee number must unique
            */
            string employeeNumber_regex = @"^T\d+$";
            Match employeeNumberCheck = Regex.Match(employeeNumber, employeeNumber_regex);

            //THIS IS NOT WORKING CORRECTLY 
            //Check for employee number is unique
            foreach (Teacher t in _teacherList)
            {
                if (t.EmployeeNumber == employeeNumber)
                {
                    employeeNumberIsUnique = false;
                    //break;
                }
                else
                {
                    employeeNumberIsUnique = true;
                    break;
                }
            }

            Console.WriteLine(firstName);
            Console.WriteLine(lastName);


            //if (firstName == "" &&  lastName == "")
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return NotFound(new { success = false, message = $"Teacher Name cannot be empty" });
            }
            //return NotFound("Teacher Name cannot be empty!");
            else if (hireDate > currentDate)
            {
                //return NotFound("Teacher Hire date cannot be in the future");
                return NotFound(new { success = false, message = $"Teacher Hire date cannot be in the future" });
            }
            else if (!employeeNumberCheck.Success)
            {
                //return NotFound("Employee Number is invalid");
                return NotFound(new { success = false, message = $"Employee Number is invalid" });
            }
            else if (!employeeNumberIsUnique)
            {
                //return NotFound("Employee Number must be unique");
                return NotFound(new { success = false, message = $"Employee Number must be unique" });
            }
            else
            {
                _db.ExecuteQuery(query);

                Console.WriteLine(query);
                //UpdateDB();
                return Ok(new { success = true, message = $"New Teacher {firstName} {lastName} is added successfully" });

                //return Ok("Teacher added successfully.");
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="ID">The ID of the teacher to be deleted.</param>
        /// <returns></returns>
        /// <example>
        /// curl -X PUT "http://localhost:5074/api/teacherapi/teacheredit?teacherID=21" -H "Content-Type: application/json" -k -d '{
        ///   "Id": 21,
        ///   "EmployeeNumber": "T0123456",
        ///   "FirstName": "Bahif",
        ///   "LastName": "sdf",
        ///   "HireDate": "1111-11-11T00:00:00",
        ///   "Salary": 50000.00
        /// }'
        /// 
        ///
        /// </example>
        [HttpPut("teacheredit")]
        public IActionResult EditTeacher(int teacherID, [FromBody] Teacher editTeacher)
        {

            bool teacherFound = false;
            
            DateTime currentDate = DateTime.Now;
            Console.WriteLine($"Editing teacher : {teacherID}");

            //THIS IS NOT WORKING CORRECTLY 
            foreach (Teacher t in _teacherList)
            {
                if(t.Id == teacherID)
                {
                    teacherFound = true;
                    break;
                }
                else{
                    teacherFound = false;
                }
            }
            if (teacherFound)
            {
                if(string.IsNullOrWhiteSpace(editTeacher.FirstName) || string.IsNullOrWhiteSpace(editTeacher.LastName))
                {
                    return NotFound(new { success = false, message = $"Teacher {teacherID} Name cannot be empty" });
                }
                else if(editTeacher.HireDate > currentDate)
                {
                    return NotFound(new { success = false, message = $"Teacher {teacherID} Hire Date cannot be in the future" });
                }
                else if (editTeacher.Salary < 0)
                {
                    return NotFound(new { success = false, message = $"Teacher {teacherID} Salary cannot be less than zero" });
                }
                else{
                    string query = $@"
                                    UPDATE teachers 
                                    SET teacherfname = '{editTeacher.FirstName}', 
                                        teacherlname = '{editTeacher.LastName}', 
                                        hiredate = '{editTeacher.HireDate:yyyy-MM-dd}', 
                                        salary = {editTeacher.Salary} 
                                    WHERE employeenumber = '{editTeacher.EmployeeNumber}'";

                    _db.ExecuteQuery(query);

                    //return Ok();
                    return Ok(new { success = true, message = $"Teacher {teacherID} got editted" });
                }
            }
            else
            {
                return NotFound(new { success = false, message = $"Teacher {teacherID} not found" });
            }
        }



        /// <summary>
        /// Deletes a teacher from the database by their teacher ID.
        /// </summary>
        /// <param name="ID">The ID of the teacher to be deleted.</param>
        /// <returns>A success or failure message depending on whether the teacher exists or not in the database.</returns>
        /// <example>
        /// curl -X DELETE "http://192.168.2.18:5000/api/teacherapi/teacherdelete?ID=14"
        /// DELETE /api/teacher/teacherdelete?ID=1
        /// Response:
        /// {
        ///     "success": true,
        ///     "message": "Teacher 1 got deleted"
        /// }
        /// </example>
        [HttpDelete("teacherdelete")]
        public IActionResult DeleteTeacher([FromQuery] int ID)
        {
            UpdateDB();
            string query = $@"DELETE FROM teachers WHERE teacherid = {ID}";

            bool _checkTeacheID = false;

            //foreach(Teacher t in _teacherList)
            for (int i = 0; i < _teacherList.Count; i++)
            {
                //if (t.Id == ID)
                if (_teacherList[i].Id == ID)
                {
                    _checkTeacheID = true;
                    break;
                }
                else
                {
                    _checkTeacheID = false;
                }
            }

            if (_checkTeacheID)
            {
                _db.ExecuteQuery(query);
                //Remove from teacher list  
                //return Ok($@"Teacher {ID} got delted");
                // Return success response in JSON format
                //UpdateDB();
                return Ok(new { success = true, message = $"Teacher {ID} got deleted" });

            }
            else
            {
                //return NotFound($@"Teacher {ID} Not Found");
                return NotFound(new { success = false, message = $"Teacher {ID} Not Found" });
            }
        }
    }
}
