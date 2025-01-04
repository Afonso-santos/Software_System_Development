using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data
{
    public class StudentDAO
    {
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
                var courseQuery = @"INSERT IGNORE INTO Course 
                                          (Name) 
                                          VALUES 
                                          (@CourseName)";

                using (var userCommand = new MySqlCommand(courseQuery, connection, transaction))
                {
                    userCommand.Parameters.AddWithValue("@CourseCode", student.Course);
                    userCommand.Parameters.AddWithValue("@CourseName", student.Course);
                    userCommand.ExecuteNonQuery();
                }

                var userDAO = UserDAO.GetInstance();
                var user = new User(student.Username, student.Username, admin: false);

                userDAO.InsertUser(user);

                var studentQuery = @"INSERT INTO Student 
                                             (Num, Name, Email, Statute, Year, Course, PartialMean, Username) 
                                             VALUES 
                                             (@Num, @Name, @Email, @Statute, @Year, @Course, @PartialMean, @Username)";

                using (var studentCommand = new MySqlCommand(studentQuery, connection, transaction))
                {
                    studentCommand.Parameters.AddWithValue("@Num", student.Number);
                    studentCommand.Parameters.AddWithValue("@Name", student.Name);
                    studentCommand.Parameters.AddWithValue("@Email", student.Email);
                    studentCommand.Parameters.AddWithValue("@Statute", student.Statute ? 1 : 0);
                    studentCommand.Parameters.AddWithValue("@Year", student.Year);
                    studentCommand.Parameters.AddWithValue("@Course", student.Course);
                    studentCommand.Parameters.AddWithValue("@PartialMean", student.PartialMean);
                    studentCommand.Parameters.AddWithValue("@Username", student.Username);
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
                        Convert.ToSingle(reader["PartialMean"]),
                        reader["Username"].ToString()!
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
                    Convert.ToSingle(reader["PartialMean"]),
                    reader["Username"].ToString()!
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
                                  Year = @Year, Semester = @Semester, Course = @Course, 
                                  PartialMean = @PartialMean, Username = @Username 
                              WHERE Num = @Num";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Num", student.Number);
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@Statute", student.Statute ? 1 : 0);
            command.Parameters.AddWithValue("@Year", student.Year);
            command.Parameters.AddWithValue("@Course", student.Course);
            command.Parameters.AddWithValue("@PartialMean", student.PartialMean);
            command.Parameters.AddWithValue("@Username", student.Username);

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

        /// <summary>
        /// Enrolls a student in a shift.
        /// </summary>
        /// <param name="studentNumber">The student's unique number.</param>
        /// <param name="shiftNumber">The shift number the student is being enrolled in.</param>
        public List<string> GetStudentKeysForShift(int shiftNumber)
        {
            var studentKeys = new List<string>();

            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"
                    SELECT Student
                    FROM Enrollment
                    WHERE Shift = @ShiftNum";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ShiftNum", shiftNumber);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    studentKeys.Add(reader["Student"].ToString()!);
                }
            }

            return studentKeys;
        }

        /// <summary>
        /// Enrolls a student in a shift by adding an entry to the Enrollment table.
        /// </summary>
        public void EnrollStudentInShift(string studentKey, string courseCode, int shiftNumber)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"
                    INSERT INTO Enrollment (Student, UC, Shift)
                    VALUES (@Student, @CourseCode, @ShiftNum)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Student", studentKey);
            command.Parameters.AddWithValue("@CourseCode", courseCode);
            command.Parameters.AddWithValue("@ShiftNum", shiftNumber);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Unenrolls a student from a shift by removing the entry from the Enrollment table.
        /// </summary>
        public void UnenrollStudentFromShift(string studentKey, string courseCode, int shiftNumber)
        {
            using var connection = DAOConfig.GetConnection();
            connection.Open();
            var query = @"
                    DELETE FROM Enrollment
                    WHERE Student = @Student AND UC = @CourseCode AND Shift = @ShiftNum";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Student", studentKey);
            command.Parameters.AddWithValue("@CourseCode", courseCode);
            command.Parameters.AddWithValue("@ShiftNum", shiftNumber);

            command.ExecuteNonQuery();
        }
    }
}
