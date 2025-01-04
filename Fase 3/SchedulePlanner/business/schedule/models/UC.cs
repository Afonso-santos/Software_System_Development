namespace SchedulePlanner.business.schedule.models
{
    public class UC(string code, string name, string courseCode, int courseYear, int semester, string? preference = null)
    {
        public string Code { get; } = code;
        public string Name { get; } = name;
        public string CourseCode { get; } = courseCode;
        public string? Preference { get; } = preference;

        public int CourseYear { get; } = courseYear;

        public int Semester { get; } = semester;

        public override string ToString() =>
            $"{Code} - {Name} (Course: {CourseCode}, Preference: {Preference ?? "None"}) - Year: {CourseYear}, Semester: {Semester}";

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
