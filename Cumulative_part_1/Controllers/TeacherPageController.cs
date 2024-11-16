using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TeacherApp.Models;

namespace TeacherApp.Controllers
{
    // API route to handle teacher-related actions
    [Route("api/[controller]")]
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext _db;
        // List to store teacher objects
        private readonly List<Teacher> _teacherList = [];
        
        // List to store course objects
        private readonly List<Course> _coursesList = [];
        
        // Configure JSON serialization with indented formatting
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        /// <summary>
        /// Constructor to initialize TeacherPageController with a SchoolDbContext instance.
        /// Retrieves teachers and courses from the database and assigns courses to teachers.
        /// </summary>
        /// <param name="db">The SchoolDbContext to interact with the database.</param>
        public TeacherPageController(SchoolDbContext db)
        {
            // Initialize the database context
            _db = db;

            // Query to get teacher data
            var query = ("SELECT * FROM  teachers" );
            
            //If Connection is Available 
            {
                var dataTable = _db.ExecuteQuery(query);
                if (dataTable.Rows.Count > 0)
                {
                    // Loop through the rows in the teachers' table
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Create a new teacher instance
                        Teacher _teacher = new();
                        // Loop through the columns
                        foreach (DataColumn column in dataTable.Columns) 
                        {

                            String columnString = column.ToString();
                            
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
                
                // Query to get course data
                query = ("SELECT * FROM  courses" );
                dataTable = _db.ExecuteQuery(query);
                
                if (dataTable.Rows.Count > 0)
                {
                    // Loop through the rows in the courses' table
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Create a new course instance
                        Course _course = new();
                        // Loop through the columns
                        foreach (DataColumn column in dataTable.Columns)
                        {

                            String columnString = column.ToString();
                            
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
                        // Add course to the list
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
                    
            }
            // else
            {
            //    Console.WriteLine("DB is offline"); 
            }
        }
        
        [HttpGet("info")]
        public IActionResult Info()
        {
            // if(SchoolDbContext.IsMySQLavailable)
            {
                var mysqlVersion = _db.GetMySqlVersion();
                Console.WriteLine($"MySQL version: {mysqlVersion}");
                return Ok(mysqlVersion); 
            }
            // else{
            //     return Ok("DB is offline");
            // }
        }
        
        [HttpGet("list")]
        public IActionResult TeacherInfo()
        {
            // Serialize to JSON
            string json = JsonSerializer.Serialize(_teacherList, _jsonOptions);

            // Pass the JSON to the view
            ViewData["TeachersJson"] = json;

            // Return the view called 'show'
            return View("List");
        }
        
        [HttpPost("list")]
        public IActionResult TeacherInfo([FromForm] string startRange, [FromForm] string endRange)
        {
            // Teacher? result = null;
            List<Teacher> resultList= [];
             // Try parsing the start and end dates
            if (DateTime.TryParse(startRange, out DateTime startDate) && DateTime.TryParse(endRange, out DateTime endDate))
            {
                foreach(Teacher t in _teacherList)
                {
                    if(t.HireDate >= startDate && t.HireDate <= endDate)
                    {
                        // result = t;
                        resultList.Add(t);
                    }
                }
                
                    string json = JsonSerializer.Serialize(resultList, _jsonOptions);
                    
                    // Pass the JSON to the view
                    ViewData["StartRange"] = startRange;
                    ViewData["EndRange"] = endRange;
                    //ViewData["TeachersJson"] = json;
                    //ViewData["TeachersJson"] = resultList.Count > 0 ? json : "Teacher not found";
                    ViewData["TeachersJson"] = resultList.Count > 0 ? json : "Teacher not found";

                    return View("List");
                
                // if(resultList.Count == 0 )
                // {
                //     ViewData["TeachersJson"] = "Teacher not found";
                //     return View("List");

                // }else{
                //     string json = JsonSerializer.Serialize(resultList, _jsonOptions);
                    
                //     // Pass the JSON to the view
                //     ViewData["StartRange"] = startRange;
                //     ViewData["EndRange"] = endRange;
                //     //ViewData["TeachersJson"] = json;
                //     ViewData["TeachersJson"] = resultList.Count > 0 ? json : "Teacher not found";

                //     return View("List");
                // }
            }
            else {
                // Serialize to JSON
                string json = JsonSerializer.Serialize(_teacherList, _jsonOptions);

                // Pass the JSON to the view
                ViewData["TeachersJson"] = json;

                return View("List");
            }
        }
        
        [HttpGet("show")]
        public IActionResult AllTeacherInfo()
        {
            return View("Show");
        }
        [HttpPost("show")]
        public IActionResult AllTeacherInfo([FromForm] string teacherId)
        {
            // Try to parse the teacherId from the form input
            if (int.TryParse(teacherId, out int number))
            {
                // If parsing is successful, search for the teacher by their ID
                Teacher? result = null;
                foreach(Teacher t in _teacherList)
                {
                    if(t.Id == number)
                    {
                        result = t;
                        break; // Exit the loop once the teacher is found
                    }
                }

                ViewData["TeacherID"] = teacherId;
                string json = JsonSerializer.Serialize(result, _jsonOptions);
                ViewData["TeacherInfo"] = result != null ? json : "Teacher not found"; // Store the teacher info in ViewData to display in the view
                // // Prepare the response to display in the view
                // if (result == null)
                // {
                //     // If the teacher is not found, set an error message in ViewData
                //     ViewData["TeacherInfo"] = "Teacher not found";
                // }
                // else
                // {
                //     // If the teacher is found, serialize the teacher data to JSON and store it in ViewData
                //     string json = JsonSerializer.Serialize(result, _jsonOptions);
                //     ViewData["TeacherInfo"] = json; // Store the teacher info in ViewData to display in the view
                // }
            }
            else
            {
                // If parsing fails, set an error message indicating invalid input
                ViewData["TeacherInfo"] = "Invalid teacher ID format";
            }

            // Return the view with the teacher info or error message
            return View("Show");
        }
    }
}
