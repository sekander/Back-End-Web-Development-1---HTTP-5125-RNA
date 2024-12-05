namespace TeacherApp.Models
{
    /// <summary>
    /// Represents a teacher in the application. 
    /// This class is designed to hold information about a teacher, 
    /// including their personal details, employment details, and the courses they teach.
    /// </summary>
    [Serializable]  // Indicates that instances of this class can be serialized for storage or transfer.
    public class Teacher
    {
        /// <summary>
        /// Gets or sets the ID for the teacher.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the employee number assigned to the teacher.
        /// The question mark (?) indicates that this property is nullable, meaning it can hold null if no value is assigned.
        /// </summary>
        public string? EmployeeNumber { get; set; }
        /// <summary>
        /// Gets or sets the first name of the teacher.
        /// nullable
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name of the teacher.
        /// nullable
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Gets or sets the date when the teacher was hired.
        /// </summary>
        public DateTime HireDate {get; set;}
        /// <summary>
        /// Gets or sets the salary of the teacher.
        /// </summary>
        public decimal Salary {get; set;}
        /// <summary>
        /// Gets or sets the list of courses that the teacher is currently teaching.
        /// </summary>        
        public List<Course> Courses { get; set; }
    
        public Teacher()
        {
            Courses = [];  // Initialize the list in the constructor
        }
    }

    /// <summary>
    /// Represents a course in the application.
    /// This class is designed to store details about a specific course taught by a teacher.
    /// </summary>
    [Serializable]  // This makes the class serializable
    public class Course
    {
        /// <summary>
        /// Gets or sets the unique identifier for the course.
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the teacher who teaches this course.
        /// This establishes a relationship between the course and the teacher.
        /// </summary>
        public int TeacherId { get; set; }
         /// <summary>
        /// Gets or sets the name of the course.
        /// nullable
        /// </summary>
        public string? CourseName { get; set; }
        /// <summary>
        /// Gets or sets the unique code assigned to the course, such as "MATH101".
        /// nullable
        /// </summary>
        public string? CourseCode { get; set; }
        /// <summary>
        /// Gets or sets the start date of the course.
        /// This indicates when the course begins.
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date of the course.
        /// This indicates when the course concludes.
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Gets or sets the number of credits assigned to the course.
        /// This is typically used to determine the weight or importance of the course.
        /// </summary>
        public int Credits { get; set; }
    }
}