namespace SchedulePlanner.business.authentication.services;
public class LoginService : interfaces.ILoginFacade
{
    private readonly IDictionary<string, (string Password, bool isAdmin)> _users;

    public LoginService(IDictionary<string, (String Password, bool isAdmin)> users)
    {
        _users = users;
    }

    public bool Login(string username, string password)
    {
        return _users.ContainsKey(username) && _users[username].Password == password;
    }

    public bool IsAdmin(string username)
    {
        return _users.ContainsKey(username) && _users[username].isAdmin;
    }
}
