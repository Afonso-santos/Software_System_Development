namespace SchedulePlanner.business.schedule.interfaces;

using SchedulePlanner.business.schedule.models;

public interface ISchedulePlanner
{
    bool StudentExists(string studentId);
    bool ShiftExists(string shiftId);
    bool HasStudents();
    bool HasClassroom(string classroomNumber);
    bool HasClassrooms();
    bool HasShifts();
    bool HasShiftsWithStudents();
    IEnumerable<string> GetStudents();
    IEnumerable<string> GetShifts();
    IEnumerable<string> GetClassrooms();
    void AddStudent(Student student);
    void AddStudentToShift(string studentId, string shiftId);
    void RemoveStudentFromShift(string studentId, string shiftId);
    void AddClassroom(Classroom classroom);
    void RemoveClassroom(string classroomId);
    void ChangeShiftClassroom(string shiftId, string classroomId);
    IEnumerable<string> GetStudentsInShift(string shiftId);
    Student FindStudent(string studentId);
    void RemoveStudent(string studentId);
    void RemoveShift(string shiftId);
    void AddShift(Shift shift);
    void UpdateStudent(Student student);
    void UpdateShift(Shift shift);
    void UpdateClassroom(Classroom classroom);
    IEnumerable<string> GetShiftsInClassroom(string classroomId);
    
}

