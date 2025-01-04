namespace SchedulePlanner.business.schedule.models;

public class Student(string number, string name, string email, bool statute, int year, string course, float partialMean, string username)
{
    public string Number { get; } = number;
    public string Name { get; } = name;
    public string Email { get; } = email;
    public bool Statute { get; } = statute;
    public int Year { get; } = year;
    public string Course { get; } = course;
    public float PartialMean { get; } = partialMean;
    public string Username { get; } = username;

    public override string ToString() => $"{Number} - {Name}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        var student = (Student)obj;
        return Number == student.Number;
    }

    public override int GetHashCode() => Number.GetHashCode();

    public static bool operator ==(Student? a, Student? b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

        return a.Number == b.Number;
    }

    public static bool operator !=(Student? a, Student? b) => !(a == b);
}

