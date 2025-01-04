using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data
{
    public class CourseDAO
    {
        private static CourseDAO? _instance;

        // Private constructor to prevent instantiation
        private CourseDAO() { }

        /// <summary>
        /// Public method to retrieve the single instance of UserDAO.
        /// </summary>
        /// <returns>The singleton instance of UserDAO.</returns>

        public static CourseDAO GetInstance()
        {
            _instance ??= new CourseDAO();
            return _instance;
        }

        // Retrieve a course by its name
        public Course? GetCourseByName(string name)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT * FROM Course WHERE Name = @Name";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Course(
                    reader["Name"].ToString()!
                );
            }

            return null; // No course found
        }

        // Retrieve all courses
        public List<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "SELECT * FROM Course";

                using var command = new MySqlCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    courses.Add(new Course(
                        reader["Name"].ToString()!
                    ));
                }
            }

            return courses;
        }

        // Add a new course
        public void InsertCourse(Course course)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"INSERT INTO Course (Name) VALUES (@Name)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", course.Name);

            command.ExecuteNonQuery();
        }

        // Update an existing course
        public void UpdateCourse(Course course)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"UPDATE Course  SET Name = @Name WHERE Name = @Name";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", course.Name);

            command.ExecuteNonQuery();
        }

        // Delete a course by its name
        public void DeleteCourse(string name)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "DELETE FROM Course WHERE Name = @Name";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.ExecuteNonQuery();
        }

        // Check if a course exists by its name
        public bool CourseExists(string name)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT COUNT(*) FROM Course WHERE Name = @Name";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }
}
