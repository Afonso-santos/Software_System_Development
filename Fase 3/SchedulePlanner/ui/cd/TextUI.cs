namespace SchedulePlanner.ui.cd;

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
        Console.WriteLine("#### Course Director Menu ####");
        this.MainMenu();
    }

    private void MainMenu()
    {
        var menu = new Menu(new[]
        {
            "Student Operations",
            "Shift Operations",
            "Classroom Operations",
            "Add Student to Shift",
            "Remove Student from Shift",
            "List Students in Shift"
        });

        // menu.SetPreCondition(3, () => this.model.HasClassrooms());
        // menu.SetPreCondition(4, () => this.model.HasStudents() && this.model.HasShifts());
        // menu.SetPreCondition(5, () => this.model.HasShiftsWithStudents());

        menu.SetHandler(1, () => ManageStudents());
        menu.SetHandler(2, () => ManageShifts());
        menu.SetHandler(3, () => ManageClassrooms());
        menu.SetHandler(4, () => AddStudentToShift());
        menu.SetHandler(5, () => RemoveStudentFromShift());
        menu.SetHandler(6, () => ListStudentsInShift());

        menu.Run(isMainMenu: true);
    }

    private void ManageStudents()
    {
        var menu = new Menu("Student Management", new[]
        {
            "Add Student",
            "View Student",
            "List Students"
        });

        menu.SetHandler(1, () => AddStudent());
        menu.SetHandler(2, () => ViewStudent());
        menu.SetHandler(3, () => ListStudents());

        menu.Run();
    }

    private void AddStudent()
    {
        try
        {
            Console.WriteLine("New student number: ");
            string? num = Console.ReadLine();

            if (num != null && !this.model.StudentExists(num))
            {
                Console.WriteLine("New student name: ");
                string? name = Console.ReadLine();
                if (name == null)
                {
                    Console.WriteLine("Student name cannot be empty.");
                    return;
                }

                Console.WriteLine("New student email: ");
                string? email = Console.ReadLine();
                if (email == null)
                {
                    Console.WriteLine("Student email cannot be empty.");
                    return;
                }

                Console.WriteLine("New student statute: ");
                string? statuteInput = Console.ReadLine();
                if (statuteInput == null || !bool.TryParse(statuteInput, out bool statute))
                {
                    Console.WriteLine("Student statute must be a valid boolean.");
                    return;
                }

                Console.WriteLine("New student year: ");
                string? yearInput = Console.ReadLine();
                if (yearInput == null || !int.TryParse(yearInput, out int year))
                {
                    Console.WriteLine("Student year cannot be empty.");
                    return;
                }

                Console.WriteLine("New student course: ");
                string? course = Console.ReadLine();
                if (course == null)
                {
                    Console.WriteLine("Student course cannot be empty.");
                    return;
                }

                Console.WriteLine("New student partial mean: ");
                string? partialMeanInput = Console.ReadLine();
                if (partialMeanInput == null || !float.TryParse(partialMeanInput, out float partialMean))
                {
                    Console.WriteLine("Student partial mean must be a valid float.");
                    return;
                }

                this.model.AddStudent(new Student(num, name, email, statute, year, course, partialMean));
                Console.WriteLine("Student added");
            }
            else
            {
                Console.WriteLine("This student number already exists!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void ViewStudent()
    {
        try
        {
            Console.WriteLine("Number to view: ");
            string? num = Console.ReadLine();
            if (num != null && this.model.StudentExists(num))
            {
                var student = model.FindStudent(num);
                Console.WriteLine(student?.ToString());
            }
            else
            {
                Console.WriteLine("This student number does not exist!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void ListStudents()
    {
        try
        {
            Console.WriteLine(string.Join("\n", this.model.GetStudents()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void ManageShifts()
    {
        var menu = new Menu("Shift Management", new[]
        {
            "Add Shift",
            "Change Shift Classroom",
            "List Shifts"
        });

        menu.SetHandler(1, () => AddShift());
        menu.SetHandler(2, () => ChangeShiftClassroom());
        menu.SetHandler(3, () => ListShifts());

        menu.Run();
    }

    private void AddShift()
    {
        Console.WriteLine("Adding a new shift...");
        // TODO Specific implementation
    }

    private void ChangeShiftClassroom()
    {
        try
        {
            Console.WriteLine("Shift number to update: ");
            string? shiftNumber = Console.ReadLine();
            if (shiftNumber != null && this.model.ShiftExists(shiftNumber))
            {
                Console.WriteLine("New classroom number: ");
                string? classroomNumber = Console.ReadLine();
                if (classroomNumber != null && this.model.HasClassroom(classroomNumber))
                {
                    this.model.ChangeShiftClassroom(shiftNumber, classroomNumber);
                    Console.WriteLine("Shift classroom updated successfully.");
                }
                else
                {
                    Console.WriteLine("The specified classroom does not exist.");
                }
            }
            else
            {
                Console.WriteLine("The specified shift does not exist.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error updating shift classroom: " + e.Message);
        }
    }

    private void ListShifts()
    {
        Console.WriteLine("Listing shifts...");
        // Specific implementation
    }

    private void ManageClassrooms()
    {
        var menu = new Menu("Classroom Management", new[]
        {
            "Add Classroom",
            "Remove Classroom",
            "List Classrooms"
        });

        menu.SetHandler(1, () => AddClassroom());
        menu.SetHandler(2, () => RemoveClassroom());
        menu.SetHandler(3, () => ListClassrooms());

        menu.Run();
    }

    private void AddClassroom()
    {
        try
        {
            Console.WriteLine("Classroom number: ");
            string? classroomNumber = Console.ReadLine();
            if (classroomNumber != null && !this.model.HasClassroom(classroomNumber))
            {
                Console.WriteLine("Capacity: ");
                string? capacity = Console.ReadLine();
                if (capacity == null)
                {
                    Console.WriteLine("Capacity cannot be empty.");
                    return;
                }

                this.model.AddClassroom(new Classroom(classroomNumber, capacity));
                Console.WriteLine("Classroom added successfully.");
            }
            else
            {
                Console.WriteLine("This classroom already exists!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error adding classroom: " + e.Message);
        }
    }

    private void RemoveClassroom()
    {
        try
        {
            Console.WriteLine("Classroom number to remove: ");
            string? classroomNumber = Console.ReadLine();
            if (classroomNumber != null && this.model.HasClassroom(classroomNumber))
            {
                this.model.RemoveClassroom(classroomNumber);
                Console.WriteLine("Classroom removed successfully.");
            }
            else
            {
                Console.WriteLine("This classroom does not exist!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error removing classroom: " + e.Message);
        }
    }

    private void ListClassrooms()
    {
        try
        {
            Console.WriteLine(string.Join("\n", this.model.GetClassrooms()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void AddStudentToShift()
    {
        try
        {
            Console.WriteLine("Student number: ");
            string? studentNumber = Console.ReadLine();
            if (studentNumber == null)
            {
                Console.WriteLine("Student number cannot be empty.");
                return;
            }

            Console.WriteLine("Shift number: ");
            string? shiftNumber = Console.ReadLine();
            if (shiftNumber == null)
            {
                Console.WriteLine("Shift number cannot be empty.");
                return;
            }

            this.model.AddStudentToShift(studentNumber, shiftNumber);
            Console.WriteLine("Student added to shift successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error adding student to shift: " + e.Message);
        }
    }

    private void RemoveStudentFromShift()
    {
        try
        {
            Console.WriteLine("Student number: ");
            string? studentNumber = Console.ReadLine();
            if (studentNumber == null)
            {
                Console.WriteLine("Student number cannot be empty.");
                return;
            }

            Console.WriteLine("Shift number: ");
            string? shiftNumber = Console.ReadLine();
            if (shiftNumber == null)
            {
                Console.WriteLine("Shift number cannot be empty.");
                return;
            }

            this.model.RemoveStudentFromShift(studentNumber, shiftNumber);
            Console.WriteLine("Student removed from shift successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error removing student from shift: " + e.Message);
        }
    }

    private void ListStudentsInShift()
    {
        try
        {
            Console.WriteLine("Shift number: ");
            string? shiftNumber = Console.ReadLine();
            if (shiftNumber == null)
            {
                Console.WriteLine("Shift number cannot be empty.");
                return;
            }

            IEnumerable<string> students = this.model.GetStudentsInShift(shiftNumber);
            Console.WriteLine(string.Join("\n", students));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error listing students in shift: " + e.Message);
        }
    }
}
