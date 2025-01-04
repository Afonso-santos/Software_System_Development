namespace SchedulePlanner.business.schedule.interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using SchedulePlanner.business.schedule.models;
using SchedulePlanner.Data;
using Newtonsoft.Json;
using static SchedulePlanner.business.schedule.models.Shift;

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

    public bool ShiftExists(string uc, ShiftType type, int number) => _shifts.ShiftExists(uc, type, number);

    public bool HasStudents() => false;

    public bool HasClassroom(string classroomNumber)
    {
        return _classrooms.ClassroomExists(classroomNumber);
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
        _classrooms.InsertClassroom(classroom);
    }

    public void RemoveClassroom(string classroomNumber)
    {
        _classrooms.DeleteClassroom(classroomNumber);
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
        _shifts.InsertShift(shift);
    }

    public void RemoveShift(string uc, ShiftType type, int number)
    {
        if (!_shifts.ShiftExists(uc, type, number))
        {
            throw new ArgumentException("Shift not found.");
        }
        _shifts.DeleteShift(uc, type, number);
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

    public void RemoveStudent(string studentNum)
    {
        if (!_students.ContainsKey(studentNum))
        {
            throw new ArgumentException("Student not found.");
        }
        _students.DeleteStudent(studentNum);
    }

    public void UpdateStudent(Student student)
    {
        if (!_students.ContainsKey(student.Number))
        {
            throw new ArgumentException("Student not found.");
        }
        _students.UpdateStudent(student);
    }

    public void UpdateShift(Shift shift)
    {
        if (!_shifts.ShiftExists(shift.UCCode, shift.Type, shift.Number))
        {
            throw new ArgumentException("Shift not found.");
        }
        _shifts.UpdateShift(shift);
    }

    public void UpdateClassroom(Classroom classroom)
    {
        if (!_classrooms.ClassroomExists(classroom.Number))
        {
            throw new ArgumentException("Classroom not found.");
        }
        _classrooms.UpdateClassroom(classroom);
    }

    public IEnumerable<string> GetShiftsInClassroom(string classroomNumber)
    {
        if (!_classrooms.ClassroomExists(classroomNumber))
        {
            throw new ArgumentException("Classroom not found.");
        }

        return _shifts.GetAllShifts().Where(s => s.ClassroomNumber == classroomNumber).Select(s => s.ToString());
    }


    public bool ImportShifts(string filePath)
    {
        throw new NotImplementedException();
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




