namespace SchedulePlanner.ui;

public class Menu{

    // Auxiliary interfaces

    /// <summary>
    /// Delegate for handlers.
    /// </summary>
    public delegate void Handler();

    /// <summary>
    /// Delegate for pre-conditions.
    /// </summary>
    public delegate bool PreCondition();

    // Class variable to support reading
    private static readonly TextReader input = Console.In;

    // Instance variables
    private readonly string title; // Menu title (optional)
    private readonly List<string> options; // List of options
    private readonly List<PreCondition> available; // List of pre-conditions
    private readonly List<Handler> handlers; // List of handlers

    // Constructors

    /// <summary>
    /// Empty constructor for Menu objects.
    /// Creates an empty menu, to which options can be added.
    /// </summary>
    public Menu()
    {
        title = "Menu";
        options = new List<string>();
        available = new List<PreCondition>();
        handlers = new List<Handler>();
    }

    /// <summary>
    /// Constructor for Menu objects.
    /// Creates a menu of options without event handlers.
    /// </summary>
    /// <param name="title">The menu title</param>
    /// <param name="options">A list of strings with the menu options.</param>
    public Menu(string title, List<string> options)
    {
        this.title = title;
        this.options = new List<string>(options);
        available = new List<PreCondition>();
        handlers = new List<Handler>();
        this.options.ForEach(s => {
            available.Add(() => true);
            handlers.Add(() => Console.WriteLine("\nWARNING: Option not implemented!"));
        });
    }

    /// <summary>
    /// Constructor for Menu objects.
    /// Creates a menu of options without event handlers.
    /// </summary>
    /// <param name="options">A list of strings with the menu options.</param>
    public Menu(List<string> options) : this("Menu", options) { }

    /// <summary>
    /// Constructor for Menu objects.
    /// Creates a menu of options without event handlers from an array.
    /// </summary>
    /// <param name="title">The menu title</param>
    /// <param name="options">An array of strings with the menu options.</param>
    public Menu(string title, string[] options) : this(title, new List<string>(options)) { }

    /// <summary>
    /// Constructor for Menu objects.
    /// Creates a menu of options without event handlers from an array.
    /// </summary>
    /// <param name="options">An array of strings with the menu options.</param>
    public Menu(string[] options) : this(new List<string>(options)) { }

    // Instance methods

    /// <summary>
    /// Add options to a Menu.
    /// </summary>
    /// <param name="name">The option to display.</param>
    /// <param name="p">The option's pre-condition.</param>
    /// <param name="h">The event handler for the option.</param>
    public void Option(string name, PreCondition p, Handler h)
    {
        options.Add(name);
        available.Add(p);
        handlers.Add(h);
    }

    /// <summary>
    /// Run the menu once.
    /// </summary>
    public void RunOnce(bool isMainMenu = false)
    {
        int op;
        Show(isMainMenu);
        op = ReadOption();
        if (op > 0 && !available[op - 1]())
        {
            Console.WriteLine("Option unavailable!");
        }
        else if (op > 0)
        {
            handlers[op - 1]();
        }
    }

    /// <summary>
    /// Run the menu multiple times. Ends with option 0.
    /// </summary>
    public void Run(bool isMainMenu = false)
    {
        int op;
        do
        {
            Show(isMainMenu);
            op = ReadOption();
            if (op > 0 && !available[op - 1]())
            {
                Console.WriteLine("Option unavailable! Try again.");
            }
            else if (op > 0)
            {
                handlers[op - 1]();
            }
        } while (op != 0);
    }

    /// <summary>
    /// Method to register a pre-condition on a menu option.
    /// </summary>
    /// <param name="i">Option index (starts at 1).</param>
    /// <param name="b">Pre-condition to register.</param>
    public void SetPreCondition(int i, PreCondition b)
    {
        available[i - 1] = b;
    }

    /// <summary>
    /// Method to register a handler on a menu option.
    /// </summary>
    /// <param name="i">Option index (starts at 1).</param>
    /// <param name="h">Handler to register.</param>
    public void SetHandler(int i, Handler h)
    {
        handlers[i - 1] = h;
    }

    // Auxiliary methods

    /// <summary>
    /// Display the menu.
    /// </summary>
    private void Show(bool isMainMenu = false)
    {
        Console.WriteLine("\n *** " + title + " *** ");
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {(available[i]() ? options[i] : "---")}");
        }
        if (isMainMenu)
            Console.WriteLine("0 - Exit");
        else
            Console.WriteLine("0 - Back");
    }

    /// <summary>
    /// Read a valid option.
    /// </summary>
    private int ReadOption()
    {
        int op;
        Console.Write("Option: ");
        try
        {
            var input = Console.ReadLine();
            op = input != null ? int.Parse(input) : -1;
        }
        catch (FormatException)
        {
            op = -1;
        }
        if (op < 0 || op > options.Count)
        {
            Console.WriteLine("Invalid Option!!!");
            op = -1;
        }
        return op;
    }
}
