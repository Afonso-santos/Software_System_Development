namespace SchedulePlanner.business.authentication.interfaces;

public interface ISSAuthentication
{
    bool Login(string username, string password);
    bool IsAdmin(string username);
}
