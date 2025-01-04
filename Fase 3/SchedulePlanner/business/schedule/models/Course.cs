namespace SchedulePlanner.business.schedule.models
{
    public class Course
    {
        public string Code { get; }
        public string Name { get; }

        public Course(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public override string ToString() => $"{Code} - {Name}";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var course = (Course)obj;
            return Code == course.Code;
        }

        public override int GetHashCode() => Code.GetHashCode();

        public static bool operator ==(Course a, Course b) => a?.Code == b?.Code;

        public static bool operator !=(Course a, Course b) => a?.Code != b?.Code;
    }
}
