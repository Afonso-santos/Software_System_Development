namespace SchedulePlanner.business.authentication.models;
public class Login
{
    private static readonly Dictionary<string, string> adminCredentials = new()
    {
        { "admin", "adminPassword" }
    };

    private static readonly Dictionary<string, string> studentCredentials = new()
    {
        { "student1", "studentPassword1" },
        { "student2", "studentPassword2" }
    };
}