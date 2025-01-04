using SchedulePlanner.business.authentication.interfaces;

class Program
{
    public static void Main()
    {
        var authenticationFacade = new AuthenticationFacade();

        Console.WriteLine("Welcome to Schedule Planner!");

        Console.Write("Enter your username: ");
        var username = Console.ReadLine();

        Console.Write("Enter your password: ");
        var password = ReadPassword();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Console.WriteLine("Username or password cannot be empty.");
            return;
        }

        if (!authenticationFacade.Login(username, password))
        {
            Console.WriteLine("Invalid credentials. Please try again.");
            return;
        }

        if (authenticationFacade.IsAdmin(username))
        {
            var textUI = new SchedulePlanner.ui.cd.TextUI();
            textUI.RunMenu();
        }
        else
        {
            var textUI = new SchedulePlanner.ui.student.TextUI(username);
            textUI.RunMenu();
        }
    }

    private static string ReadPassword()
    {
        var password = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}
