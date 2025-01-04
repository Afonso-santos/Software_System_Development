using SchedulePlanner.Data;

namespace SchedulePlanner.business.schedule.models
{
    public class UC
    {
        public string Code { get; }
        public string Name { get; }
        public string CourseCode { get; }
        public string? Preference { get; }

        private readonly CourseDAO courseDAO;

        public UC(string code, string name, string courseCode, string? preference = null)
        {
            Code = code;
            Name = name;
            CourseCode = courseCode;
            Preference = preference;
            courseDAO = new CourseDAO();
        }

        public override string ToString() =>
            $"{Code} - {Name} (Course: {CourseCode}, Preference: {Preference ?? "None"})";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var uc = (UC)obj;
            return Code == uc.Code;
        }

        public override int GetHashCode() => Code.GetHashCode();

        public static bool operator ==(UC a, UC b) => a?.Code == b?.Code;

        public static bool operator !=(UC a, UC b) => a?.Code != b?.Code;
    }
}
