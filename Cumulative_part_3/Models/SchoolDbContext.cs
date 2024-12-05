using MySql.Data.MySqlClient;
using System.Data;

namespace TeacherApp.Models
{
    /// <summary>
    /// Represents a context for interacting with the MySQL database.
    /// This class provides methods for connecting to the database, executing queries, and fetching database information.
    /// </summary>
    public class SchoolDbContext(string connectionString)
{
        // The connection string used to establish a connection to the MySQL database.
        private readonly string _connectionString = connectionString;

        /// <summary>
        /// Initializes a new instance of the class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string used to connect to the MySQL database.</param>
        public MySqlConnection GetConnection()
        {
             // Stores the connection string for later use.
            return new MySqlConnection(_connectionString);
        }
        
        /// <summary>
        /// Creates and returns a new instance of using the stored connection string.
        /// This method is used to establish a connection to the database.
        /// </summary>
        /// <returns>A object connected to the MySQL database.</returns>
        public string? GetMySqlVersion()
        {
            // Using a 'using' statement to automatically close the connection once the operation is done.
            using var connection = GetConnection();
            try
            {
                // Opens the connection to the database.
                connection.Open();
                // Returns the server version.
                return connection.ServerVersion;
            }
            catch (Exception ex)
            {
                // Logs any errors to the console.
                Console.WriteLine($"Error getting MySQL version: {ex.Message}");
                // Returns null if an error occurs during the connection or version retrieval.
                return null;
            }
        }

        /// <summary>
        /// Executes a given SQL query against the MySQL database and returns the results as a 
        /// The is populated with the data returned by the query.
        /// </summary>
        /// <param name="query">The SQL query to execute against the database.</param>
        /// <returns>
        /// A containing the result set of the query.
        /// The will be empty if the query does not return any data or if an error occurs.
        /// </returns>
        public DataTable ExecuteQuery(string query)
        {
            // Creates a new connection to the database.
            using var connection = GetConnection();
            // Initializes a DataTable to store the results of the query.
            var dataTable = new DataTable();
            try
            {
                // Opens the connection to the database.
                connection.Open();
                // Creates a MySqlCommand object with the query and connection.
                var command = new MySqlCommand(query, connection);
                // Initializes a data adapter to fill the DataTable with query results.
                var adapter = new MySqlDataAdapter(command);
                // Executes the query and fills the DataTable with the results.
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                // Logs any errors that occur during the query execution.
                Console.WriteLine($"Error executing query: {ex.Message}");
            }
            // Returns the DataTable, which may be empty if there was no data or an error.
            return dataTable;
        }
    }
}
