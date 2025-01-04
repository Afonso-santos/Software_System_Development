using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data
{
    public class ClassroomDAO
    {
        // Retrieve a classroom by its number
        public Classroom? GetClassroomByNumber(string number)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT * FROM Classroom WHERE Number = @Number";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Number", number);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Classroom(
                    reader["Number"].ToString()!,
                    reader["Capacity"].ToString()!
                );
            }

            return null; // No classroom found
        }

        // Retrieve all classrooms
        public List<Classroom> GetAllClassrooms()
        {
            var classrooms = new List<Classroom>();
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "SELECT * FROM Classroom";

                using var command = new MySqlCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    classrooms.Add(new Classroom(
                        reader["Number"].ToString()!,
                        reader["Capacity"].ToString()!
                    ));
                }
            }

            return classrooms;
        }

        // Add a new classroom
        public void InsertClassroom(Classroom classroom)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"INSERT INTO Classroom (Number, Capacity) 
                              VALUES (@Number, @Capacity)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Number", classroom.Number);
            command.Parameters.AddWithValue("@Capacity", classroom.Capacity);

            command.ExecuteNonQuery();
        }

        // Update an existing classroom
        public void UpdateClassroom(Classroom classroom)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"UPDATE Classroom 
                              SET Capacity = @Capacity 
                              WHERE Number = @Number";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Number", classroom.Number);
            command.Parameters.AddWithValue("@Capacity", classroom.Capacity);

            command.ExecuteNonQuery();
        }

        // Delete a classroom by its number
        public void DeleteClassroom(string number)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "DELETE FROM Classroom WHERE Number = @Number";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Number", number);
            command.ExecuteNonQuery();
        }

        // Check if a classroom exists by its number
        public bool ClassroomExists(string number)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT COUNT(*) FROM Classroom WHERE Number = @Number";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Number", number);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }

        public bool Any()
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT COUNT(*) FROM Classroom";

            using var command = new MySqlCommand(query, connection);
            var count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
}
