namespace SchedulePlanner.business.schedule.models
{
    public class Course(string name)
    {
        public string Name { get; } = name;

        public override string ToString() => $"{Name} - {Name}";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var course = (Course)obj;
            return Name == course.Name;
        }

        public override int GetHashCode() => Name.GetHashCode();

        public static bool operator ==(Course a, Course b) => a?.Name == b?.Name;

        public static bool operator !=(Course a, Course b) => a?.Name != b?.Name;
    }
}
