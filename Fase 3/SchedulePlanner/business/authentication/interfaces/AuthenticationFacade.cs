namespace SchedulePlanner.business.authentication.interfaces;

using SchedulePlanner.Data;

public class AuthenticationFacade : ISSAuthentication
{
    private UserDAO _userDAO;

    public AuthenticationFacade() {
        _userDAO = UserDAO.GetInstance();
    }

    public bool Login(string username, string password)
    {
        // Get the user from the UserDAO object by his username
        try {
            var user = _userDAO.GetUserByUsername(username);
            if (user == null)
            {
                // User not found
                return false;
            }

            return user.Password == password;

        } catch (Exception e) {
            // SQL Exception
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public bool IsAdmin(string username)
    {
        try {
            var user = _userDAO.GetUserByUsername(username);
            if (user == null)
            {
                // User not found
                return false;
            }

            return user.Admin;

        } catch (Exception e) {
            // SQL Exception
            Console.WriteLine(e.Message);
            return false;
        }
    }
}
