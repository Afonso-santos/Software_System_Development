namespace SchedulePlanner.business.schedule.interfaces;

using SchedulePlanner.business.schedule.models;
using static SchedulePlanner.business.schedule.models.Shift;

public interface ISchedulePlanner
{
    bool StudentExists(string studentId);
    bool ShiftExists(string uc, ShiftType type, int number);
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
    void AllocateAllStudents();
    void RemoveStudentFromShift(string studentId, string shiftId);
    void AddClassroom(Classroom classroom);
    void RemoveClassroom(string classroomId);
    void ChangeShiftClassroom(string shiftId, string classroomId);
    IEnumerable<string> GetStudentsInShift(string shiftId);
    Student? FindStudent(string studentId);

    bool ImportStudent(string filePath);
    bool ImportShifts(string filePath, string courseName);

    void AddUCS(UC uc);

    void RemoveUCS(string code);
    IEnumerable<string> GetUCs();

    void AddCourse(Course course);
    void RemoveCourse(string code);
    IEnumerable<string> GetCourses();
    bool CourseExists(string code);

    void RemoveStudent(string studentId);
    void RemoveShift(string uc, ShiftType type, int number);
    void AddShift(Shift shift);
    void UpdateStudent(Student student);
    void UpdateShift(Shift shift);
    void UpdateClassroom(Classroom classroom);
    IEnumerable<string> GetShiftsInClassroom(string classroomId);

}
