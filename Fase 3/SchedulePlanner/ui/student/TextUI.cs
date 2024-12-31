namespace SchedulePlanner.ui.student;

using SchedulePlanner.ui;
using SchedulePlanner.business.schedule.interfaces;
using SchedulePlanner.business.schedule.models;

public class TextUI
{
    private readonly ISchedulePlanner model;

    public TextUI()
    {
        this.model = new SchedulePlannerFacade();
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
            // TODO
            Console.WriteLine("View schedule not implemented.");
            // Console.WriteLine("Student number: ");
            // var studentNumber = Console.ReadLine();
            // var schedule = this.model.GetSchedule(studentNumber);
            // Console.WriteLine(schedule);
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
            // TODO
            Console.WriteLine("Export schedule not implemented.");
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
