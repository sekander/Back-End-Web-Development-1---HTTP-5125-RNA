using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using TeacherApp.Models;

namespace TeacherApp.Controllers
{
    /// <summary>
    /// Controller responsible for handling teacher-related API requests.
    /// It retrieves teacher and course data from the database, filters teachers by hire date range,
    /// and allows displaying information about teachers and courses in various formats.
    /// </summary>
[Route("api/[controller]")]
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext _db;
        // List to store teacher objects
        private static readonly List<Teacher> _teacherList = [];
        
        // List to store course objects
        private static readonly List<Course> _coursesList = [];
        
        // Configure JSON serialization with indented formatting
        private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    
        //Makes sure if update db is only executed once in the controller constructor
        private static bool _hasExecuted = false;

        /// <summary>
        /// Constructor to initialize TeacherPageController with a SchoolDbContext instance.
        /// Retrieves teachers and courses from the database and assigns courses to teachers.
        /// </summary>
        /// <param name="db">The SchoolDbContext to interact with the database.</param>
        public TeacherPageController(SchoolDbContext db)
        {
            // Initialize the database context
            _db = db;
            if(!_hasExecuted)
            {
                UpdateDB();
                _hasExecuted = true;
            }
        }

        private void UpdateDB()
        {
            _teacherList.Clear();
            _coursesList.Clear();
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
        }
        
        /*
         * <summary>
         * Retrieves the MySQL version from the database and returns it in the response.
         * </summary>
         *
         * <returns>
         * A JSON response containing the MySQL version string.
         * </returns>
         *
         * <param name="temperature">
         * None
         * </param>
         *
         * <example>
         * GET /api/teacherpage/info
         * Returns: "8.0.23"
         * </example>
         */
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
        
        
        /*
         * <summary>
         * Retrieves the list of all teachers and returns it serialized as JSON.
         * </summary>
         *
         * <returns>
         * A view containing the list of teachers in JSON format.
         * </returns>
         *
         * <param name="temperature">
         * None
         * </param>
         *
         * <example>
         * GET /api/teacherpage/list
         * Returns a list of teachers in JSON format.
         * </example>
         */
        [HttpGet("list")]
        public IActionResult TeacherInfo()
        {
            UpdateDB();
            // Serialize to JSON
            string json = JsonSerializer.Serialize(_teacherList, _jsonOptions);

            // Pass the JSON to the view
            ViewData["TeachersJson"] = json;

            // Return the view called 'show'
            return View("List");
        }
        
         /*
         * <summary>
         * Filters the teacher list by hire date range and returns the filtered list in JSON format.
         * </summary>
         *
         * <returns>
         * A view with a list of teachers within the specified hire date range.
         * </returns>
         *
         * <param name="startRange">
         * The start date for filtering teachers by their hire date.
         * </param>
         *
         * <param name="endRange">
         * The end date for filtering teachers by their hire date.
         * </param>
         *
         * <example>
         * POST /api/teacherpage/list
         * startRange=2020-01-01&endRange=2023-12-31
         * Returns: A list of teachers hired between the specified dates.
         * </example>
         */
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
                    ViewData["TeachersJson"] = resultList.Count > 0 ? json : "Teacher not found";

                    return View("List");
                
            }
            else {
                // Serialize to JSON
                string json = JsonSerializer.Serialize(_teacherList, _jsonOptions);

                // Pass the JSON to the view
                ViewData["TeachersJson"] = json;

                return View("List");
            }
        }
        
         /*
         * <summary>
         * Displays the information for all teachers.
         * </summary>
         *
         * <returns>
         * A view displaying all teacher data.
         * </returns>
         *
         * <param name="temperature">
         * None
         * </param>
         *
         * <example>
         * GET /api/teacherpage/show
         * Returns: A view with all teacher information.
         * </example>
         */
        [HttpGet("show")]
        public IActionResult AllTeacherInfo()
        {
            
            UpdateDB();
            return View("Show");
        }
        
         /*
         * <summary>
         * Displays the information of a specific teacher based on their ID.
         * </summary>
         *
         * <returns>
         * A view displaying the teacher's details in JSON format.
         * </returns>
         *
         * <param name="teacherId">
         * The ID of the teacher to fetch information for.
         * </param>
         *
         * <example>
         * POST /api/teacherpage/show
         * teacherId=3
         * Returns: Teacher information for the teacher with ID 3.
         * </example>
         */
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
            }
            else
            {
                // If parsing fails, set an error message indicating invalid input
                ViewData["TeacherInfo"] = "Invalid teacher ID format";
            }

            // Return the view with the teacher info or error message
            return View("Show");
        }

        
        /// <summary>
        /// Displays the view for adding a new teacher.
        /// The view contains a form where the user can input teacher details.
        /// This API call is expected to be triggered via an AJAX request in the frontend,
        /// which then loads the form for adding a new teacher dynamically.
        /// </summary>
        /// <returns>
        /// A view for adding a new teacher.
        /// </returns>
        /// <example>
        /// GET /api/teacherpage/add
        /// Returns: A view with a form to add a new teacher.
        /// This view can be dynamically loaded via an AJAX request in the frontend.
        /// </example>
        [HttpGet("add")]
        public IActionResult AddTeacher()
        {

            //string json = JsonSerializer.Serialize(_teacherList, _jsonOptions);
            // Pass the JSON to the view
            //ViewData["TeachersJson"] = json;
            
            return View("New");
        }
        
        /// <summary>
        /// This method handles the HTTP GET request for the "edit" route, which is used to display a view 
        /// with a form to edit or add a teacher's information. It prepares the necessary data for the view 
        /// by serializing the list of teacher IDs and the full teacher list into JSON format, which can 
        /// then be accessed by the frontend to dynamically populate the form fields.
        /// </summary>
        /// <returns>
        /// Returns an `IActionResult` representing the view for editing a teacher. This view is rendered 
        /// with the serialized data (teacher IDs and teacher information) passed via `ViewData`.
        /// </returns>
        /// <example>
        /// GET /api/teacherpage/edit
        /// Returns: A view with a form to add a new teacher.
        /// This view can be dynamically loaded via an AJAX request in the frontend.
        /// </example>
        [HttpGet("edit")]
        public IActionResult EditTeacher()
        {
            // Call a method to update the database, possibly refreshing data or performing other operations
            UpdateDB();

            // Create a new list to store teacher IDs
            List<int> teachersID = [];
            
            // Loop through the teacher list and extract the IDs to the teachersID list
            foreach(Teacher t in _teacherList)
            {
                teachersID.Add( t.Id); // Add each teacher's ID to the list
            } 
            
            // Serialize the teacher IDs list to JSON format and store it in ViewData for use in the frontend
            string json = JsonSerializer.Serialize(teachersID, _jsonOptions);
            // Pass the JSON to the view
            ViewData["TeacherIDJson"] = json;
            
            // Serialize the full list of teachers to JSON format and store it in ViewData for the frontend
            string jsonT = JsonSerializer.Serialize(_teacherList, _jsonOptions);
            // Pass the JSON to the view
            ViewData["TeacherJson"] = jsonT;
            
            // Return the "Edit" view, which will now have access to the teacher IDs and teacher data in JSON format
            return View("Edit");
        }
        
        
        
        
        /// <summary>
        /// Displays the view for deleting a teacher.
        /// The view contains a confirmation form where the user can confirm teacher deletion.
        /// This API call is triggered through an AJAX request, which loads the deletion 
        /// confirmation dynamically. The list of teachers is passed to the view 
        /// as JSON data for further interaction (such as selecting the teacher to delete).
        /// </summary>
        /// <returns>
        /// A view to confirm teacher deletion.
        /// </returns>
        /// <example>
        /// GET /api/teacherpage/delete
        /// Returns: A view with a confirmation message to delete a teacher.
        /// This view can be dynamically loaded via an AJAX request in the frontend.
        /// </example>
        [HttpGet("delete")]
        public IActionResult DeleteTeacher()
        {

            UpdateDB();
            string json = JsonSerializer.Serialize(_teacherList, _jsonOptions);
            // Pass the JSON to the view
            ViewData["TeachersJson"] = json;
            
            return View("DeleteConfirm");
        }
    }
}
