using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SchedulePlanner.Data;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data
{
    public class CourseDAO
    {
        // Retrieve a course by its code
        public Course? GetCourseByCode(string code)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "SELECT * FROM Course WHERE Code = @Code";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Course(
                                reader["Code"].ToString()!,
                                reader["Name"].ToString()!
                            );
                        }
                    }
                }
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

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course(
                            reader["Code"].ToString()!,
                            reader["Name"].ToString()!
                        ));
                    }
                }
            }

            return courses;
        }

        // Add a new course
        public void AddCourse(Course course)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"INSERT INTO Course (Code, Name) 
                              VALUES (@Code, @Name)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", course.Code);
                    command.Parameters.AddWithValue("@Name", course.Name);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Update an existing course
        public void UpdateCourse(Course course)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = @"UPDATE Course 
                              SET Name = @Name 
                              WHERE Code = @Code";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", course.Code);
                    command.Parameters.AddWithValue("@Name", course.Name);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete a course by its code
        public void DeleteCourse(string code)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "DELETE FROM Course WHERE Code = @Code";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Check if a course exists by its code
        public bool CourseExists(string code)
        {
            using (var connection = DAOConfig.GetConnection())
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Course WHERE Code = @Code";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", code);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}
