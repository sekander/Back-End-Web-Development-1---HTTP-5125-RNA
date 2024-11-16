using System.Data;
using System.Text.Json;
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
        private readonly List<Teacher> _teacherList = [];
        // List to store courses
        private readonly List<Course> _coursesList = [];
        // Static options for JSON serialization with indentation.
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// This constructor loads teachers and courses data from the database and associates courses with the respective teachers.
        /// </summary>
        /// <param name="db">The database context used to execute queries against the database.</param>
        public TeacherApiController(SchoolDbContext db)
        {
            // Stores the database context for later use.
            _db = db;
            
            // if(!_hasExecuted)
            {
                // Query to fetch teachers' data from the database
                var query = ("SELECT * FROM  teachers" );
                var dataTable = _db.ExecuteQuery(query);
                
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
                            }
                        }
                        // Add teacher to the list
                        _teacherList.Add(_teacher);
                    }
                }
                
                // Query to fetch course data from the database
                query = ("SELECT * FROM  courses" );
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
                            }
                        }
                        // Add course to the list`
                        _coursesList.Add(_course);
                    }
                }
                    
                 // Now, assign courses to the respective teacher based on teacherid
                foreach (var teacher in _teacherList)
                {
                    // Find courses for this teacher based on teacherid
                    var teacherCourses = _coursesList.Where(c => c.TeacherId == teacher.Id).ToList();
                    
                    // Add the courses to the teacher
                    teacher.Courses.AddRange(teacherCourses);
                }
                // _hasExecuted = true;
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
            var mysqlVersion = _db.GetMySqlVersion();
            Console.WriteLine($"MySQL version: {mysqlVersion}");
            return Ok("Teacher API"); 
        }
        
        /// <summary>
        /// Retrieves all teachers' information.
        /// </summary>
        /// <returns>Returns a JSON array of teacher objects containing all teacher details.</returns>
        /// <example>
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
            var _students = new List<Dictionary<string, object>>();
            
            var query = ("SELECT * FROM  students" );
            var dataTable = _db.ExecuteQuery(query);
            
            // If there are any rows in the students data table
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var student = new Dictionary<string, object>();
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
            foreach(Teacher t in _teacherList)
            {
                if(t.Id == @teacherID)
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
                // Return the serialized JSON as the response
                return Ok(json); 
        }
    }
}