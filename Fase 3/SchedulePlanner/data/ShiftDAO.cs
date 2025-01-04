using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;
using static SchedulePlanner.business.schedule.models.Shift;

namespace SchedulePlanner.Data
{
    public class ShiftDAO
    {
        public Shift? GetShiftByNumber(string uc, ShiftType type, int shiftNumber)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();

            var query = @"SELECT Num, Type, UC, Day, StartingHour, EndingHour, `Limit`, Classroom
                              FROM Shift
                                WHERE UC = @UC AND Type = @Type AND Num = @ShiftNumber";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UC", uc);
            command.Parameters.AddWithValue("@Type", type.ToString());
            command.Parameters.AddWithValue("@ShiftNumber", shiftNumber);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ConvertReaderToShift(reader);
            }

            return null;
        }

        public List<Shift> GetShiftsByCourse(string courseCode)
        {
            var shifts = new List<Shift>();

            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"SELECT Num
                              FROM Shift
                              WHERE Course = @CourseCode";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CourseCode", courseCode);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    shifts.Add(ConvertReaderToShift(reader));
                }
            }

            return shifts;
        }

        public List<Shift> GetAllShifts()
        {
            var shifts = new List<Shift>();

            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"SELECT s.Num, s.Type, s.Day, s.StartingHour, s.EndingHour, s.`Limit`, s.UC, s.Classroom
                              FROM Shift s";

                using var command = new MySqlCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    shifts.Add(ConvertReaderToShift(reader));
                }
            }

            return shifts;
        }

        public void InsertShift(Shift shift)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"INSERT INTO Shift (Num, Type, Day, StartingHour, EndingHour, `Limit`, UC, Classroom)
                          VALUES (@Num, @Type, @Day, @StartingHour, @EndingHour, @Limit, @Course, @Classroom)";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Num", shift.Number);
            command.Parameters.AddWithValue("@Type", shift.Type.ToString());
            command.Parameters.AddWithValue("@Day", shift.Day);
            command.Parameters.AddWithValue("@StartingHour", shift.StartingHour);
            command.Parameters.AddWithValue("@EndingHour", shift.EndingHour);
            command.Parameters.AddWithValue("@Limit", shift.Capacity);
            command.Parameters.AddWithValue("@Course", shift.UCCode);
            command.Parameters.AddWithValue("@Classroom", shift.ClassroomNumber);

            command.ExecuteNonQuery();
        }

        public void DeleteShift(string uc, ShiftType type, int number)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();  
            var query = "DELETE FROM Shift WHERE UC = @UC AND Type = @Type AND Num = @ShiftNumber";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UC", uc);
            command.Parameters.AddWithValue("@Type", type.ToString());
            command.Parameters.AddWithValue("@ShiftNumber", number);

            command.ExecuteNonQuery();
        }

        public void UpdateShift(Shift shift)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"UPDATE Shift 
                              SET Type = @Type, Day = @Day, StartingHour = @StartingHour, EndingHour = @EndingHour, `Limit` = @Limit, UC = @Course, Classroom = @Classroom
                              WHERE Num = @Num";
            
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Num", shift.Number);
            command.Parameters.AddWithValue("@Type", shift.Type.ToString());
            command.Parameters.AddWithValue("@Day", shift.Day);
            command.Parameters.AddWithValue("@StartingHour", shift.StartingHour);
            command.Parameters.AddWithValue("@EndingHour", shift.EndingHour);
            command.Parameters.AddWithValue("@Limit", shift.Capacity);
            command.Parameters.AddWithValue("@Course", shift.UCCode);
            command.Parameters.AddWithValue("@Classroom", shift.ClassroomNumber);

            command.ExecuteNonQuery();
        }

        public bool ShiftExists(string uc, ShiftType type, int number)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT COUNT(*) FROM Shift WHERE UC = @UC AND Type = @Type AND Num = @ShiftNumber";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UC", uc);
            command.Parameters.AddWithValue("@Type", type.ToString());
            command.Parameters.AddWithValue("@ShiftNumber", number);

            var count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }

        public bool Any()
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT COUNT(*) FROM Shift";

            using var command = new MySqlCommand(query, connection);
            var count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }

        private Shift ConvertReaderToShift(MySqlDataReader reader)
        {
            var shiftNum = Convert.ToInt32(reader["Num"]);
            var shiftType = Enum.Parse<Shift.ShiftType>(reader["Type"].ToString()!);
            var ucCode = reader["UC"].ToString()!;

            var day = reader["Day"].ToString()!;
            var startingHour = TimeSpan.Parse(reader["StartingHour"].ToString()!);
            var endingHour = TimeSpan.Parse(reader["EndingHour"].ToString()!);

            var capacity = Convert.ToInt32(reader["Limit"]);
            var classroomNumber = reader["Classroom"].ToString()!;

            // shift.LoadStudents(); // Load students into the shift

            return new Shift(shiftNum, shiftType, day, startingHour, endingHour, capacity, ucCode, classroomNumber);
        }

    }
}
