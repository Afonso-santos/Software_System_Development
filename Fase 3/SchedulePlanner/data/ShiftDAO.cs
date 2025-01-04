using System;
using System.Collections.Generic;
using System.Globalization;
using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data
{
    public class ShiftDAO
    {
        public Shift? GetShiftByNumber(int shiftNumber)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"SELECT s.Num, s.Type, s.Day, s.Hour, s.`Limit`, s.Course, s.Classroom
                              FROM Shift s
                              WHERE s.Num = @ShiftNumber";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ShiftNumber", shiftNumber);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {   var shiftNum = Convert.ToInt32(reader["Num"]);
                            var shiftType = Enum.Parse<Shift.ShiftType>(reader["Type"].ToString()!);
                            var day = reader["Day"].ToString()!;
                            var hour = TimeSpan.Parse(reader["Hour"].ToString()!);
                            var capacity = Convert.ToInt32(reader["Limit"]);
                            var courseCode = reader["Course"].ToString()!;
                            var classroomNumber = reader["Classroom"].ToString()!;

                            var classroomDAO = new ClassroomDAO();
                            var classroom = classroomDAO.GetClassroomByNumber(classroomNumber);

                            var shift = new Shift(shiftNum, shiftType, day, hour, capacity, courseCode, classroom);

                            shift.LoadStudents(); // Load students into the shift
                            return shift;
                        }
                    }
                }
            }

            return null;
        }

        public List<Shift> GetShiftsByCourse(string courseCode)
        {
            var shifts = new List<Shift>();

            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"SELECT s.Num, s.Type, s.Day, s.Hour, s.`Limit`, s.Course, s.Classroom
                              FROM Shift s
                              WHERE s.Course = @CourseCode";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseCode", courseCode);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var shiftNumber =Convert.ToInt32(reader["Num"]);
                            var shiftType = Enum.Parse<Shift.ShiftType>(reader["Type"].ToString()!);
                            var day = reader["Day"].ToString()!;
                            var hour = TimeSpan.Parse(reader["Hour"].ToString()!);
                            var capacity = Convert.ToInt32(reader["Limit"]);
                            var classroomNumber = reader["Classroom"].ToString()!;

                            var classroomDAO = new ClassroomDAO();
                            var classroom = classroomDAO.GetClassroomByNumber(classroomNumber);

                            var shift = new Shift(shiftNumber, shiftType, day, hour, capacity, courseCode, classroom);

                            shift.LoadStudents(); // Load students into the shift
                            shifts.Add(shift);
                        }
                    }
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
                var query = @"SELECT s.Num, s.Type, s.Day, s.Hour, s.`Limit`, s.Course, s.Classroom
                              FROM Shift s";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var shiftNumber = Convert.ToInt32(reader["Num"]);
                        var shiftType = Enum.Parse<Shift.ShiftType>(reader["Type"].ToString()!);
                        var day = reader["Day"].ToString()!;
                        var hour = TimeSpan.Parse(reader["Hour"].ToString()!);
                        var capacity = Convert.ToInt32(reader["Limit"]);
                        var courseCode = reader["Course"].ToString()!;
                        var classroomNumber = reader["Classroom"].ToString()!;

                        var classroomDAO = new ClassroomDAO();
                        var classroom = classroomDAO.GetClassroomByNumber(classroomNumber);

                        var shift = new Shift(shiftNumber, shiftType, day, hour, capacity, courseCode, classroom);

                        shift.LoadStudents(); // Load students into the shift
                        shifts.Add(shift);
                    }
                }
            }

            return shifts;
        }

        public void AddShift(Shift shift)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO Shift (Num, Type, Day, Hour, `Limit`, Course, Classroom)
                              VALUES (@Num, @Type, @Day, @Hour, @Limit, @Course, @Classroom)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Num", shift.Number);
                    command.Parameters.AddWithValue("@Type", shift.Type.ToString());
                    command.Parameters.AddWithValue("@Day", shift.Day);
                    command.Parameters.AddWithValue("@Hour", shift.Hour);
                    command.Parameters.AddWithValue("@Limit", shift.Capacity);
                    command.Parameters.AddWithValue("@Course", shift.CourseCode);
                    command.Parameters.AddWithValue("@Classroom", shift.Classroom?.Number);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteShift(String shiftNumber)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "DELETE FROM Shift WHERE Num = @ShiftNumber";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ShiftNumber", shiftNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateShift(Shift shift)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"UPDATE Shift 
                              SET Type = @Type, Day = @Day, Hour = @Hour, `Limit` = @Limit, 
                                  Course = @Course, Classroom = @Classroom
                              WHERE Num = @Num";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Num", shift.Number);
                    command.Parameters.AddWithValue("@Type", shift.Type.ToString());
                    command.Parameters.AddWithValue("@Day", shift.Day);
                    command.Parameters.AddWithValue("@Hour", shift.Hour);
                    command.Parameters.AddWithValue("@Limit", shift.Capacity);
                    command.Parameters.AddWithValue("@Course", shift.CourseCode);
                    command.Parameters.AddWithValue("@Classroom", shift.Classroom?.Number);

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ContainsKey (string shiftNumber)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                int number = Convert.ToInt32(shiftNumber);
                connection.Open();
                var query = "SELECT COUNT(*) FROM Shift WHERE Num = @ShiftNumber";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ShiftNumber", number);

                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool Any()
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Shift";

                using (var command = new MySqlCommand(query, connection))
                {
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        
    }
}
