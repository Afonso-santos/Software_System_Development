using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data
{
    public class StudentDAO
    {
        private static StudentDAO? _instance;

        // Private constructor to prevent instantiation
        private StudentDAO() { }

        /// <summary>
        /// Public method to retrieve the single instance of UserDAO.
        /// </summary>
        /// <returns>The singleton instance of UserDAO.</returns>

        public static StudentDAO GetInstance()
        {
            _instance ??= new StudentDAO();
            return _instance;
        }

        private readonly ShiftDAO _shifts_DAO = ShiftDAO.GetInstance();

        /// <summary>
        /// Inserts a new student into the database.
        /// </summary>
        /// <param name="student">The student object to be inserted.</param>
        public void InsertStudent(Student student)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var courseDAO = CourseDAO.GetInstance();
                if (!courseDAO.CourseExists(student.Course))
                {
                    var course = new Course(student.Course);
                    courseDAO.InsertCourse(course);
                }

                var userDAO = UserDAO.GetInstance();
                var user = new User(student.Number, student.Number, admin: false);
                userDAO.InsertUser(user);

                var studentQuery = @"INSERT INTO Student 
                                             (Num, Name, Email, Statute, Year, Course, PartialMean) 
                                             VALUES 
                                             (@Num, @Name, @Email, @Statute, @Year, @Course, @PartialMean)";

                using (var studentCommand = new MySqlCommand(studentQuery, connection, transaction))
                {
                    studentCommand.Parameters.AddWithValue("@Num", student.Number);
                    studentCommand.Parameters.AddWithValue("@Name", student.Name);
                    studentCommand.Parameters.AddWithValue("@Email", student.Email);
                    studentCommand.Parameters.AddWithValue("@Statute", student.Statute ? 1 : 0);
                    studentCommand.Parameters.AddWithValue("@Year", student.Year);
                    studentCommand.Parameters.AddWithValue("@Course", student.Course);
                    studentCommand.Parameters.AddWithValue("@PartialMean", student.PartialMean);
                    studentCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Retrieves all students from the database.
        /// </summary>
        /// <returns>A list of students.</returns>
        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "SELECT * FROM Student";

                using var command = new MySqlCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var student = new Student(
                        reader["Num"].ToString()!,
                        reader["Name"].ToString()!,
                        reader["Email"].ToString()!,
                        Convert.ToBoolean(reader["Statute"]),
                        Convert.ToInt32(reader["Year"]),
                        reader["Course"].ToString()!,
                        Convert.ToSingle(reader["PartialMean"])
                    );
                    students.Add(student);
                }
            }

            return students;
        }

        /// <summary>
        /// Retrieves a student by their number.
        /// </summary>
        /// <param name="number">The student's unique number.</param>
        /// <returns>The student object, or null if not found.</returns>
        public Student? GetStudentByNumber(string number)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT * FROM Student WHERE Num = @Num";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Num", number);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Student(
                    reader["Num"].ToString()!,
                    reader["Name"].ToString()!,
                    reader["Email"].ToString()!,
                    Convert.ToBoolean(reader["Statute"]),
                    Convert.ToInt32(reader["Year"]),
                    reader["Course"].ToString()!,
                    Convert.ToSingle(reader["PartialMean"])
                );
            }

            return null;
        }

        /// <summary>
        /// Updates an existing student's information in the database.
        /// </summary>
        /// <param name="student">The student object with updated information.</param>
        public void UpdateStudent(Student student)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"UPDATE Student 
                              SET Name = @Name, Email = @Email, Statute = @Statute, 
                                  Year = @Year, Course = @Course, 
                                  PartialMean = @PartialMean 
                              WHERE Num = @Num";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Num", student.Number);
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@Statute", student.Statute ? 1 : 0);
            command.Parameters.AddWithValue("@Year", student.Year);
            command.Parameters.AddWithValue("@Course", student.Course);
            command.Parameters.AddWithValue("@PartialMean", student.PartialMean);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes a student from the database by their number.
        /// </summary>
        /// <param name="number">The student's unique number.</param>
        public void DeleteStudent(string number)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var studentQuery = "DELETE FROM Student WHERE Num = @Num";
                using (var studentCommand = new MySqlCommand(studentQuery, connection, transaction))
                {
                    studentCommand.Parameters.AddWithValue("@Num", number);
                    studentCommand.ExecuteNonQuery();
                }

                var userQuery = "DELETE FROM User WHERE Username = (SELECT Username FROM Student WHERE Num = @Num)";
                using (var userCommand = new MySqlCommand(userQuery, connection, transaction))
                {
                    userCommand.Parameters.AddWithValue("@Num", number);
                    userCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<Shift> GetStudentEnrollments(string studentNumber)
        {
            var shifts = new List<Shift>();

            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"SELECT ShiftNum, ShiftType, ShiftUC
                              FROM Enrollment
                              WHERE Student = @Student";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Student", studentNumber);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var shiftNumber = Convert.ToInt32(reader["ShiftNum"]);
                    var shiftType = Enum.Parse<Shift.ShiftType>(reader["ShiftType"].ToString()!);
                    var shiftUC = reader["ShiftUC"].ToString()!;

                    var shift = _shifts_DAO.GetShift(shiftUC, shiftType, shiftNumber);
                    if (shift is not null)
                    {
                        shifts.Add(shift!);
                    }
                }
            }

            return shifts;
        }

        public bool ContainsKey(string key)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = "SELECT * FROM Student WHERE Num = @Num";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Num", key);

            using (var reader = command.ExecuteReader())
            {
                return reader.Read();
            }
        }
    }
}
