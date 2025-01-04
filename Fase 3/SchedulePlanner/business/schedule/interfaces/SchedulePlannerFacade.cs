namespace SchedulePlanner.business.schedule.interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Security;
using SchedulePlanner.business.schedule.models;
using SchedulePlanner.Data;
using Newtonsoft.Json;

public class SchedulePlannerFacade : ISchedulePlanner
{
    private readonly StudentDAO _students;
    private readonly ClassroomDAO _classrooms;
    private readonly ShiftDAO _shifts;

    private readonly CourseDAO _courses;

    private readonly UCDAO _ucs;

    public SchedulePlannerFacade()
    {
        _students = new StudentDAO();
        _classrooms = new ClassroomDAO();
        _shifts = new ShiftDAO();
        _courses = CourseDAO.GetInstance();
        _ucs = new UCDAO();
    }

    public bool StudentExists(string num) => _students.ContainsKey(num);

    public bool ShiftExists(string num) => _shifts.ContainsKey(num);

    public bool HasStudents() => false;

    public bool HasClassroom(string classroomNumber)
    {
        return _classrooms.ContainsKey(classroomNumber);
    }

    public bool HasClassrooms() => _classrooms.Any();

    public bool HasShifts() => _shifts.Any();

    public bool HasShiftsWithStudents() => _shifts.GetAllShifts().Any(s => s.StudentKeys.Any());

    public IEnumerable<string> GetStudents() => _students.GetAllStudents().Select(s => s.ToString());

    public IEnumerable<string> GetShifts() => _shifts.GetAllShifts().Select(s => s.ToString());

    public IEnumerable<string> GetClassrooms() => _classrooms.GetAllClassrooms().Select(c => c.ToString());

    public void AddStudent(Student student)
    {
        _students.InsertStudent(student);
    }

    public void AddStudentToShift(string studentNum, string shiftNum)
    {

    }

    public void RemoveStudentFromShift(string studentNum, string shiftNum)
    {

    }

    public void AddClassroom(Classroom classroom)
    {
    }

    public void RemoveClassroom(string classroomNumber)
    {

    }

    public void ChangeShiftClassroom(string shiftNum, string classroomNum)
    {

    }

    public IEnumerable<string> GetStudentsInShift(string shiftNum)
    {
        throw new NotImplementedException();
    }

    public void AddShift(Shift shift)
    {

    }

    public void RemoveShift(string shiftNumber)
    {

    }

    public void SetShiftClassroom(string shiftNum, string classroomNum)
    {

    }

    public void SetShiftCapacity(string shiftNum, string capacity)
    {


    }

    public void SetShiftType(string shiftNum, string type)
    {

    }

    public Student? FindStudent(string num)
    {
        return _students.GetStudentByNumber(num);
    }


    /// <summary>
    /// Imports students, courses, and curricular units (UCs) from a CSV file.
    /// </summary>
    /// <param name="fileName">The path to the CSV file.</param>
    /// <returns>True if the operation succeeds, false otherwise.</returns>
    public bool ImportStudentsAndUCs(string fileName)
    {
        try
        {
            using (var reader = new StreamReader(fileName))
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    throw new InvalidOperationException("The file is empty.");
                }

                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');


                    if (fields.Length < 16)
                    {
                        Console.WriteLine($"Invalid line: {line}");
                        continue;
                    }

                    // Retrieves course information.
                    var courseCode = fields[3];
                    var courseName = fields[4];

                    // Ensures the course exists in the database; adds it if not.
                    if (!_courses.CourseExists(courseCode))
                    {
                        var course = _courses.GetCourseByName(courseCode);
                        course = new Course(courseCode);
                        _courses.InsertCourse(course);
                    }

                    // Retrieves UC (Curricular Unit) information.
                    var ucCode = fields[7];
                    var ucName = fields[8];
                    var yearOfUC = Convert.ToInt32(fields[6]);

                    // Ensures the UC exists in the database; adds it if not.
                    var uc = _ucs.GetUCByCode(ucCode);
                    if (uc is null)
                    {
                        // uc = new UC(ucCode, ucName, courseCode, null);
                        // _ucs.AddUC(uc);
                    }


                    var studentNumber = fields[11];
                    var studentName = fields[12];
                    var studentEmail = fields[13];
                    var specialRegime = fields[15];

                    // Determines if the student has a special regime.
                    var statute = !string.IsNullOrWhiteSpace(specialRegime);

                    // Creates a new student object.
                    var student = new Student(
                        studentNumber,
                        studentName,
                        studentEmail,
                        statute,
                        year: yearOfUC, // Sets the student's year based on the UC.
                        course: courseCode,
                        partialMean: 0.0f // Default value.
                    );

                    // Inserts or updates the student in the database.
                    var existingStudent = _students.GetStudentByNumber(studentNumber);
                    if (existingStudent == null)
                    {
                        _students.InsertStudent(student);
                    }
                    else
                    {
                        _students.UpdateStudent(student);
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing students and UCs: {ex.Message}");
            return false;
        }

    }

    /*
        public void RemoveStudent(string studentNum)
        {
            if (!_students.ContainsKey(studentNum))
            {
                throw new ArgumentException("Student not found.");
            }
            _students.Remove(studentNum);
        }

        public void RemoveShift(string shiftNum)
        {
            if (!_shifts.ContainsKey(shiftNum))
            {
                throw new ArgumentException("Shift not found.");
            }
            _shifts.Remove(shiftNum);
        }

        public void AddShift(Shift shift)
        {
            if (_shifts.ContainsKey(shift.Number))
            {
                throw new ArgumentException("Shift already exists.");
            }
            _shifts[shift.Number] = shift;
        }

        public void UpdateStudent(Student student)
        {
            if (!_students.ContainsKey(student.Number))
            {
                throw new ArgumentException("Student not found.");
            }
            _students[student.Number] = student;
        }

        public void UpdateShift(Shift shift)
        {
            if (!_shifts.ContainsKey(shift.Number))
            {
                throw new ArgumentException("Shift not found.");
            }
            _shifts[shift.Number] = shift;
        }

        public void UpdateClassroom(Classroom classroom)
        {
            if (!_classrooms.ContainsKey(classroom.Number))
            {
                throw new ArgumentException("Classroom not found.");
            }
            _classrooms[classroom.Number] = classroom;
        }

        public IEnumerable<string> GetShiftsInClassroom(string classroomNumber)
        {
            if (!_classrooms.ContainsKey(classroomNumber))
            {
                throw new ArgumentException("Classroom not found.");
            }

            return _shifts.Values.Where(s => s.Classroom.Number == classroomNumber).Select(s => s.ToString());
        }

        */

    public bool ImportShifts(string filePath)
    {
        try
        {
            var jsonData = File.ReadAllText(filePath);
            var shiftsData = JsonConvert.DeserializeObject<List<ShiftData>>(jsonData);

            if (shiftsData == null)
            {
                Console.WriteLine("No shift data found in the file.");
                return false;
            }

            foreach (var shiftData in shiftsData)
            {
                if (shiftData == null)
                {
                    Console.WriteLine("Invalid shift data.");
                    continue;
                }


                var filterId = shiftData.FilterId.ToString();
                var year = int.Parse(filterId.Substring(0, 1));
                var semester = int.Parse(filterId.Substring(1, 1));
                var uniqueId = int.Parse(filterId.Substring(2));

                if (string.IsNullOrEmpty(shiftData.Shift))
                {
                    Console.WriteLine("Invalid shift data: Shift is null or empty.");
                    continue;
                }
                var shiftType = Enum.Parse<Shift.ShiftType>(shiftData.Shift.Substring(0, 2));
                var shiftNumber = uniqueId;
                var day = GetDayOfWeek(shiftData.Day);
                if (string.IsNullOrEmpty(shiftData.Start) || string.IsNullOrEmpty(shiftData.End))
                {
                    Console.WriteLine("Invalid shift data: Start time is null or empty.");
                    continue;
                }
                var startHour = TimeSpan.Parse(shiftData.Start);
                var endHour = TimeSpan.Parse(shiftData.End);
                var courseCode = shiftData.Id;
                var building = shiftData.Building;
                var room = shiftData.Room;
                var capacity = 0;

                if (string.IsNullOrEmpty(room))
                {
                    Console.WriteLine("Invalid shift data: Room is null or empty.");
                    continue;
                }

                if (string.IsNullOrEmpty(building))
                {
                    Console.WriteLine("Invalid shift data: Building is null or empty.");
                    continue;
                }

                if (shiftData.Theoretical)
                {
                    capacity = 100;
                }
                else
                {
                    capacity = 50;
                }

                if (string.IsNullOrEmpty(courseCode))
                {
                    Console.WriteLine("Invalid shift data: Course code is null or empty.");
                    continue;
                }
                var classroom = new Classroom(room, building, capacity.ToString());
                var shift = new Shift(shiftNumber, shiftType, day, startHour, capacity, courseCode, classroom);

                var existingShift = _shifts.GetShiftByNumber(shiftNumber);
                if (existingShift is null)
                {
                    _shifts.AddShift(shift);
                }
                else
                {
                    _shifts.UpdateShift(shift);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing shifts: {ex.Message}");
            return false;
        }
    }

    private string GetDayOfWeek(int day)
    {
        return day switch
        {
            0 => "Monday",
            1 => "Tuesday",
            2 => "Wednesday",
            3 => "Thursday",
            4 => "Friday",
            5 => "Saturday",
            6 => "Sunday",
            _ => throw new ArgumentOutOfRangeException(nameof(day), "Invalid day of the week")
        };
    }

    private class ShiftData
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public bool Theoretical { get; set; }
        public string? Shift { get; set; }
        public string? Building { get; set; }
        public string? Room { get; set; }
        public int Day { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public int FilterId { get; set; }
    }
}




