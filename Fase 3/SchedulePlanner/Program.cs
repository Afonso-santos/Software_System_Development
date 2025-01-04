using SchedulePlanner.business.authentication.services;

class Program
{
    public static void Main()
    {
        var loginService = new LoginService();

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

        if (!loginService.Login(username, password))
        {
            Console.WriteLine("Invalid credentials. Please try again.");
            return;
        }

        if (loginService.IsAdmin(username))
        {
            var textUI = new SchedulePlanner.ui.cd.TextUI();
            textUI.RunMenu();
        }
        else
        {
            var textUI = new SchedulePlanner.ui.student.TextUI();
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
