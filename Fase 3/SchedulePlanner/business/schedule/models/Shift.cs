namespace SchedulePlanner.business.schedule.models;

public class Shift
{
    public string Number { get; }
    public Classroom? Classroom { get; set; }
    public List<Student> Students { get; }

    public Shift(string number)
    {
        Number = number;
        Students = new List<Student>();
    }

    public void AddStudent(Student student)
    {
        if (Students.Exists(s => s.Number == student.Number))
        {
            throw new ArgumentException("Student is already in the shift.");
        }
        Students.Add(student);
    }

    public void RemoveStudent(string studentNumber)
    {
        var student = Students.FirstOrDefault(s => s.Number == studentNumber);
        if (student == null)
        {
            throw new ArgumentException("Student not found in the shift.");
        }

        Students.Remove(student);
    }

    public override string ToString() => $"{Number} - Classroom: {Classroom?.Number ?? "None"}";

    public enum ShiftType
    {
        T,  // Theoretical
        TP, // Theoretical-Practical
        PL  // Practical-Laboratory
    }

    public ShiftType Type { get; set; }

    public int Capacity { get; set; }

    public int NumberOfStudents => Students.Count;

    public bool IsFull => NumberOfStudents >= Capacity;

    public bool IsEmpty => NumberOfStudents == 0;

    public bool CanAddStudent => !IsFull;

    public bool CanRemoveStudent => !IsEmpty;
}
