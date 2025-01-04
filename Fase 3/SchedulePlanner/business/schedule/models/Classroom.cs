namespace SchedulePlanner.business.schedule.models;

public class Classroom
{
    public string Number { get; }
    public string Capacity { get; }

    public Classroom(string number, string capacity)
    {
        Number = number;
        Capacity = capacity;
    }

    public override string ToString() => $"{Number} - {Capacity}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        var classroom = (Classroom)obj;
        return Number == classroom.Number;
    }

    // Used to compare two objects of the same type

    public override int GetHashCode() => Number.GetHashCode();

    // Compare two classrooms by their number
    public static bool operator ==(Classroom a, Classroom b) => a?.Number == b?.Number;

    public static bool operator !=(Classroom a, Classroom b) => a?.Number != b?.Number;
}
