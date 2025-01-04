namespace SchedulePlanner.Data;

using MySql.Data.MySqlClient;

public static class DAOConfig
{
    public const string Username = "admin"; 
    public const string Password = "admin123"; 
    private const string Database = "SchedulePlanner";
    private const string Server = "localhost";
    private const int Port = 3306;
    public static readonly string ConnectionString = $"Server={Server};Port={Port};Database={Database};User ID={Username};Password={Password};";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }

    public static void CloseConnection(MySqlConnection connection)
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
