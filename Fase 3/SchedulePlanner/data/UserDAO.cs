using MySql.Data.MySqlClient;
using SchedulePlanner.business.schedule.models;

namespace SchedulePlanner.Data;

public class UserDAO
{
    // Private instance of the singleton
    private static UserDAO? _instance;

    // Private constructor to prevent instantiation
    private UserDAO() { }

    /// <summary>
    /// Public method to retrieve the single instance of UserDAO.
    /// </summary>
    /// <returns>The singleton instance of UserDAO.</returns>
    public static UserDAO GetInstance()
    {
        if (_instance == null)
        {
            _instance = new UserDAO();
        }
        return _instance;
    }

    /// <summary>
    /// Inserts a new user into the database.
    /// </summary>
    /// <param name="user">The User object to be inserted.</param>
    public void InsertUser(User user)
    {
        using (var connection = DAOConfig.GetConnection())
        {
            connection.Open();
            var query = @"INSERT INTO User (Username, Password, Admin) VALUES (@Username, @Password, @Admin)";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Admin",    user.Admin);

                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Retrieves a user based on their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>The User object if found, otherwise null.</returns>
    public User? GetUserByUsername(string username)
    {
        using (var connection = DAOConfig.GetConnection())
        {
            connection.Open();
            var query = "SELECT * FROM User WHERE Username = @Username";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User(
                            reader["Username"].ToString()!,
                            reader["Password"].ToString()!,
                            reader["Admin"].ToString() == "1"  // Convert to boolean value
                        );
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Updates the password of an existing user.
    /// </summary>
    /// <param name="user">The User object with the updated password.</param>
    public  void UpdateUser(User user)
    {
        using (var connection = DAOConfig.GetConnection())
        {
            connection.Open();
            var query = @"UPDATE User SET Password = @Password WHERE Username = @Username";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);

                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Deletes a user from the database based on their username.
    /// </summary>
    /// <param name="username">The username of the user to be deleted.</param>
    public static void DeleteUser(string username)
    {
        using (var connection = DAOConfig.GetConnection())
        {
            connection.Open();
            var query = "DELETE FROM User WHERE Username = @Username";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.ExecuteNonQuery();
            }
        }
    }
}
