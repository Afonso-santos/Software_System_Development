namespace SchedulePlanner.business.schedule.models
{
    public class Shift(int number, Shift.ShiftType type, string day, TimeSpan startingHour, TimeSpan endingHour, int capacity, string ucCode, string classroomNumber)
    {
        public int Number { get; } = number;
        public ShiftType Type { get; } = type;
        public string UCCode { get; } = ucCode;
        public string Day { get; } = day;
        public TimeSpan StartingHour { get; } = startingHour;
        public TimeSpan EndingHour { get; } = endingHour;
        public int Capacity { get; } = capacity;
        public string ClassroomNumber { get; } = classroomNumber;

        public List<string> StudentKeys { get; } = new List<string>();

        public int NumberOfStudents => StudentKeys.Count;
        public bool IsFull => NumberOfStudents >= Capacity;
        public bool IsEmpty => NumberOfStudents == 0;
        public bool CanAddStudent => !IsFull;
        public bool CanRemoveStudent => !IsEmpty;

        public enum ShiftType
        {
            T,  // Theoretical
            TP, // Theoretical-Practical
            PL  // Practical-Laboratory
        }

        // Add student to the shift
        public void EnrollStudent(string studentKey)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Cannot add student. Shift is already full.");
            }
            if (StudentKeys.Contains(studentKey))
            {
                throw new ArgumentException("Student is already in the shift.");
            }

            StudentKeys.Add(studentKey);
            // enroll the student on the DAO
        }

        // Remove student from the shift
        public void RemoveStudent(string studentKey)
        {
            if (!StudentKeys.Contains(studentKey))
            {
                throw new ArgumentException("Student not found in the shift.");
            }

            StudentKeys.Remove(studentKey);
            // unroll the student on the DAO
        }

        public override string ToString() =>
            $"Shift {Number}: {Type} on {Day} from {StartingHour} to {EndingHour} in Classroom {ClassroomNumber} with {NumberOfStudents}/{Capacity} students. UC: {UCCode}";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var shift = (Shift)obj;
            return Number == shift.Number;
        }

        public override int GetHashCode() => Number.GetHashCode();

        public static bool operator ==(Shift a, Shift b) => a?.Number == b?.Number;

        public static bool operator !=(Shift a, Shift b) => a?.Number != b?.Number;
    }
}
