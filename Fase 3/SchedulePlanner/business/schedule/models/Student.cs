namespace SchedulePlanner.business.schedule.models;

public class Student
{
    public string Number { get; }
    public string Name { get; }
    public string Email { get; }

    public Student(string number, string name, string email)
    {
        Number = number;
        Name = name;
        Email = email;
    }

    public override string ToString() => $"{Number} - {Name}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        var student = (Student) obj;
        return Number == student.Number;
    }

    // Used to compare two objects of the same type
    public override int GetHashCode() => Number.GetHashCode();

    // Compare two students by their number
    public static bool operator ==(Student? a, Student? b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

        return a.Number == b.Number;
    }

    public static bool operator !=(Student? a, Student? b)
    {
        return !(a == b);
    }
}
