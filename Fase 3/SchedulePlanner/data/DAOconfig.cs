namespace SchedulePlanner.Data;

public static class DAOConfig
{
    public const string Username = "root";
    public const string Password = "root";
    private const string Database = "scheduleplanner";
    private const string Driver = "jdbc:mysql";
    public static readonly string Url = $"{Driver}://localhost:3306/{Database}";
}
