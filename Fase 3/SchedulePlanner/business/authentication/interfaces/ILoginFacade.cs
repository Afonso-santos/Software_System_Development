namespace SchedulePlanner.business.authentication.interfaces;
public interface ILoginFacade
{
    bool Login(string username, string password);

    bool IsAdmin(string username);
}
