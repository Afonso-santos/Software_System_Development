namespace SchedulePlanner.ui.student;

using SchedulePlanner.ui;
using SchedulePlanner.business.schedule.interfaces;

public class TextUI
{
    private readonly ISchedulePlanner model;
    private readonly string username;

    public TextUI(string username)
    {
        this.model = new SchedulePlannerFacade();
        this.username = username;
    }

    public void RunMenu()
    {
        Console.WriteLine("#### Student Menu ####");
        var menu = new Menu(new[]
        {
            "View Schedule",
            "Export Schedule"
        });

        menu.SetHandler(1, () => ViewSchedule());
        menu.SetHandler(2, () => ExportSchedule());
        menu.Run(isMainMenu: true);
    }

    private void ViewSchedule()
    {
        try
        {
            var schedule = this.model.GetSchedule(username);

            Console.WriteLine("\nYour calendar:");
            Console.WriteLine(schedule);

        }
        catch (Exception e)
        {
            Console.WriteLine("Error viewing schedule: " + e.Message);
        }
    }

    private void ExportSchedule()
    {
        try
        {
            Console.WriteLine("Export schedule not implemented. Similar Feature to View Schedule.");
            // Console.WriteLine("Student number: ");
            // var studentNumber = Console.ReadLine();
            // Console.WriteLine("File name: ");
            // var fileName = Console.ReadLine();
            // this.model.ExportSchedule(studentNumber, fileName);
            // Console.WriteLine("Schedule exported successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error exporting schedule: " + e.Message);
        }
    }

}
