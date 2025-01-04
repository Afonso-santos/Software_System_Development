namespace SchedulePlanner.ui.cd;

using SchedulePlanner.ui;
using SchedulePlanner.business.schedule.interfaces;
using SchedulePlanner.business.schedule.models;
using static SchedulePlanner.business.schedule.models.Shift;

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
            "List Students in Shift",
            "Import Schedules"
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
        menu.SetHandler(7, () => ImportSchedules());

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
            "List Shifts",
            "Remove Shift"
        });

        menu.SetHandler(1, () => AddShift());
        menu.SetHandler(2, () => ListShifts());
        menu.SetHandler(3, () => RemoveShift());

        menu.Run();
    }

    private void AddShift()
    {
        Console.WriteLine("Adding a new shift...");

        try
        {
            Console.WriteLine("New shift UC: ");
            string? uc = Console.ReadLine();

            Console.WriteLine("New shift type (0: T; 1: TP; 2: PL): ");
            string? typeInput = Console.ReadLine()?.ToUpper();
            if (typeInput == null || !Enum.TryParse<ShiftType>(typeInput, out ShiftType type))
            {
                Console.WriteLine("Shift type must be a valid type.");
                return;
            }

            Console.WriteLine("New shift number: ");
            string? numInput = Console.ReadLine();
            if (numInput == null || !int.TryParse(numInput, out int num))
            {
                Console.WriteLine("Shift number must be a valid integer.");
                return;
            }

            if (uc != null && !model.ShiftExists(uc, type, num))
            {
                Console.WriteLine("New shift week day: (monday, tuesday, wednesday, thursday, friday)");
                string? day = Console.ReadLine()?.ToLower();
                if (day == null || day != "monday" && day != "tuesday" && day != "wednesday" && day != "thursday" && day != "friday")
                {
                    Console.WriteLine("Shift week day cannot be empty.");
                    return;
                }

                Console.WriteLine("New shift start hour: ");
                string? startHourInput = Console.ReadLine();
                if (startHourInput == null || !TimeSpan.TryParse(startHourInput , out TimeSpan startHour))
                {
                    Console.WriteLine("Shift start hour must be a valid time.");
                    return;
                }

                Console.WriteLine("New shift ending hour: ");
                string? endHourInput = Console.ReadLine();
                if (endHourInput == null || !TimeSpan.TryParse(endHourInput, out TimeSpan endHour))
                {
                    Console.WriteLine("Shift ending hour must be a valid time.");
                    return;
                }

                Console.WriteLine("New shift capacity: ");
                string? capacityInput = Console.ReadLine();
                if (capacityInput == null || !int.TryParse(capacityInput, out int capacity))
                {
                    Console.WriteLine("Shift capacity must be a valid integer.");
                    return;
                }

                Console.WriteLine("New shift classroom number: ");
                string? classroomNumber = Console.ReadLine();
                if (classroomNumber == null || !model.HasClassroom(classroomNumber))
                {
                    Console.WriteLine("Shift classroom number must be a valid classroom.");
                    return;
                }

                model.AddShift(new Shift(num, type, day, startHour, endHour, capacity, uc, classroomNumber));
                Console.WriteLine("Shift added");
            }
            else
            {
                Console.WriteLine("This shift already exists!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    } 

    private void ListShifts()
    {
        Console.WriteLine(string.Join("\n", this.model.GetShifts()));
    }

    private void RemoveShift()
    {
        try
        {
            Console.WriteLine("Shift UC: ");
            string? uc = Console.ReadLine();

            Console.WriteLine("Shift type (0: T; 1: TP; 2: PL): ");
            string? typeInput = Console.ReadLine()?.ToUpper();
            if (typeInput == null || !Enum.TryParse<ShiftType>(typeInput, out ShiftType type))
            {
                Console.WriteLine("Shift type must be a valid type.");
                return;
            }

            Console.WriteLine("Shift number: ");
            string? numInput = Console.ReadLine();
            if (numInput == null || !int.TryParse(numInput, out int num))
            {
                Console.WriteLine("Shift number must be a valid integer.");
                return;
            }

            if (uc != null && this.model.ShiftExists(uc, type, num))
            {
                this.model.RemoveShift(uc, type, num);
                Console.WriteLine("Shift removed successfully.");
            }
            else
            {
                Console.WriteLine("This shift does not exist!");
            }


        }
        catch (Exception e)
        {
            Console.WriteLine("Error removing shift: " + e.Message);
        }
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

    private void ImportSchedules()
    {
        try
        {
            Console.Write("File path: ");
            string? filePath = Console.ReadLine();
            if (filePath == null)
            {
                Console.WriteLine("File path cannot be empty.");
                return;
            }

            if (this.model.ImportShifts(filePath))
            {
                Console.WriteLine("Schedules imported successfully.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error importing schedules: " + e.Message);
        }
    }
}
