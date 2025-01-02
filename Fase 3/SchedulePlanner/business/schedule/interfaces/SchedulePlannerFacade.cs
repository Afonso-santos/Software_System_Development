namespace SchedulePlanner.business.schedule.interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using SchedulePlanner.business.schedule.models;

public class SchedulePlannerFacade : ISchedulePlanner
{
    private readonly Dictionary<string, Student> _students;
    private readonly Dictionary<string, Classroom> _classrooms;
    private readonly Dictionary<string, Shift> _shifts;

    public SchedulePlannerFacade()
    {
        _students = new Dictionary<string, Student>();
        _classrooms = new Dictionary<string, Classroom>();
        _shifts = new Dictionary<string, Shift>();
    }

    public bool StudentExists(string num) => _students.ContainsKey(num);

    public bool ShiftExists(string num) => _shifts.ContainsKey(num);

    public bool HasStudents() => _students.Any();

    public bool HasClassroom(string classroomNumber) {
        return _classrooms.ContainsKey(classroomNumber);
    }

    public bool HasClassrooms() => _classrooms.Any();

    public bool HasShifts() => _shifts.Any();

    public bool HasShiftsWithStudents() => _shifts.Values.Any(s => s.Students.Any());

    public IEnumerable<string> GetStudents() => _students.Values.Select(s => s.ToString());

    public IEnumerable<string> GetShifts() => _shifts.Values.Select(s => s.ToString());

    public IEnumerable<string> GetClassrooms() => _classrooms.Values.Select(c => c.ToString());

    public void AddStudent(Student student)
    {
        if (_students.ContainsKey(student.Number))
        {
            throw new ArgumentException("Student already exists.");
        }
        _students[student.Number] = student;
    }

    public void AddStudentToShift(string studentNum, string shiftNum)
    {
        if (!_students.ContainsKey(studentNum))
        {
            throw new ArgumentException("Student not found.");
        }

        if (!_shifts.ContainsKey(shiftNum))
        {
            throw new ArgumentException("Shift not found.");
        }

        _shifts[shiftNum].AddStudent(_students[studentNum]);
    }

    public void RemoveStudentFromShift(string studentNum, string shiftNum)
    {
        if (!_shifts.ContainsKey(shiftNum))
        {
            throw new ArgumentException("Shift not found.");
        }

        _shifts[shiftNum].RemoveStudent(studentNum);
    }

    public void AddClassroom(Classroom classroom)
    {
        if (_classrooms.ContainsKey(classroom.Number))
        {
            throw new ArgumentException("Classroom already exists.");
        }
        _classrooms[classroom.Number] = classroom;
    }

    public void RemoveClassroom(string classroomNumber)
    {
        if (!_classrooms.ContainsKey(classroomNumber))
        {
            throw new ArgumentException("Classroom not found.");
        }
        _classrooms.Remove(classroomNumber);
    }

    public void ChangeShiftClassroom(string shiftNum, string classroomNum)
    {
        if (!_shifts.ContainsKey(shiftNum))
        {
            throw new ArgumentException("Shift not found.");
        }

        if (!_classrooms.ContainsKey(classroomNum))
        {
            throw new ArgumentException("Classroom not found.");
        }

        _shifts[shiftNum].Classroom = _classrooms[classroomNum];
    }

    public IEnumerable<string> GetStudentsInShift(string shiftNum)
    {
        if (!_shifts.ContainsKey(shiftNum))
        {
            throw new ArgumentException("Shift not found.");
        }

        return _shifts[shiftNum].Students.Select(s => s.ToString());
    }

    public void AddShift(Shift shift)
    {
        if (_shifts.ContainsKey(shift.Number))
        {
            throw new ArgumentException("Shift already exists.");
        }
        _shifts[shift.Number] = shift;
    }

    public void RemoveShift(string shiftNumber)
    {
        if (!_shifts.ContainsKey(shiftNumber))
        {
            throw new ArgumentException("Shift not found.");
        }
        _shifts.Remove(shiftNumber);
    }

    public void SetShiftClassroom(string shiftNum, string classroomNum)
    {
        if (!_shifts.ContainsKey(shiftNum))
        {
            throw new ArgumentException("Shift not found.");
        }

        if (!_classrooms.ContainsKey(classroomNum))
        {
            throw new ArgumentException("Classroom not found.");
        }

        _shifts[shiftNum].Classroom = _classrooms[classroomNum];
    }

    public void SetShiftCapacity(string shiftNum, string capacity)
    {
        if (!_shifts.ContainsKey(shiftNum))
        {
            throw new ArgumentException("Shift not found.");
        }

        if (!int.TryParse(capacity, out var cap))
        {
            throw new ArgumentException("Invalid capacity.");
        }

        _shifts[shiftNum].Capacity = cap;
    }

    public void SetShiftType(string shiftNum, string type)
    {
        if (!_shifts.ContainsKey(shiftNum))
        {
            throw new ArgumentException("Shift not found.");
        }

        if (!Enum.TryParse<Shift.ShiftType>(type, out var shiftType))
        {
            throw new ArgumentException("Invalid shift type.");
        }

        _shifts[shiftNum].Type = shiftType;
    }

    public Student FindStudent(string num)
    {
        if (!_students.ContainsKey(num))
        {
            throw new ArgumentException("Student not found.");
        }

        return _students[num];
    }

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

}
