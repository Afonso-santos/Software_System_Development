using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data
{
    public class UCDAO
    {
        // Retrieve a UC by its code
        public UC? GetUCByCode(string code)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT * FROM UC WHERE Code = @Code";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Code", code);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UC(
                    reader["Code"].ToString()!,
                    reader["Name"].ToString()!,
                    reader["Course"].ToString()!,
                    Convert.ToInt32(reader["CourseYear"]),
                    Convert.ToInt32(reader["Semester"]),
                    reader["Preference"]?.ToString()
                );
            }

            return null; // No UC found
        }

        // Retrieve a UC by its name
        public UC? GetUCByName(string name)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT * FROM UC WHERE Name = @Name";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", name);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UC(
                    reader["Code"].ToString()!,
                    reader["Name"].ToString()!,
                    reader["Course"].ToString()!,
                    Convert.ToInt32(reader["CourseYear"]),
                    Convert.ToInt32(reader["Semester"]),
                    reader["Preference"]?.ToString()
                );
            }

            return null; // No UC found
        }

        // Retrieve all UCs
        public List<UC> GetAllUCs()
        {
            var ucs = new List<UC>();
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "SELECT * FROM UC";

                using var command = new MySqlCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ucs.Add(new UC(
                        reader["Code"].ToString()!,
                        reader["Name"].ToString()!,
                        reader["Course"].ToString()!,
                        Convert.ToInt32(reader["CourseYear"]),
                        Convert.ToInt32(reader["Semester"]),
                        reader["Preference"]?.ToString()
                    ));
                }
            }

            return ucs;
        }

        // Add a new UC
        public void InsertUC(UC uc)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"INSERT INTO UC (Code, Name, Course, Preference, CourseYear, Semester) 
                              VALUES (@Code, @Name, @Course, @Preference, @CourseYear, @Semester)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Code", uc.Code);
            command.Parameters.AddWithValue("@Name", uc.Name);
            command.Parameters.AddWithValue("@Course", uc.CourseCode);
            command.Parameters.AddWithValue("@Preference", uc.Preference);
            command.Parameters.AddWithValue("@CourseYear", uc.CourseYear);
            command.Parameters.AddWithValue("@Semester", uc.Semester);

            command.ExecuteNonQuery();
        }

        // Update an existing UC
        public void UpdateUC(UC uc)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"UPDATE UC 
                              SET Name = @Name, Course = @Course, Preference = @Preference, CourseYear = @CourseYear, Semester = @Semester
                              WHERE Code = @Code";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Code", uc.Code);
            command.Parameters.AddWithValue("@Name", uc.Name);
            command.Parameters.AddWithValue("@Course", uc.CourseCode);
            command.Parameters.AddWithValue("@Preference", uc.Preference);
            command.Parameters.AddWithValue("@CourseYear", uc.CourseYear);
            command.Parameters.AddWithValue("@Semester", uc.Semester);

            command.ExecuteNonQuery();
        }

        // Delete a UC by its code
        public void DeleteUC(string code)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "DELETE FROM UC WHERE Code = @Code";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Code", code);
            command.ExecuteNonQuery();
        }

        // Check if a UC exists by its code
        public bool UCExists(string code)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT COUNT(*) FROM UC WHERE Code = @Code";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Code", code);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }
}
