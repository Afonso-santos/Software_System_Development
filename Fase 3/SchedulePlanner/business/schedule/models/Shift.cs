using System;
using System.Collections.Generic;
using SchedulePlanner.Data;

namespace SchedulePlanner.business.schedule.models
{
    public class Shift
    {
        public int Number { get; } // Changed from string to int
        public Classroom? Classroom { get; set; }
        public List<string> StudentKeys { get; }
        private readonly StudentDAO studentDAO;

        public ShiftType Type { get; set; }
        public int Capacity { get; set; }
        public string Day { get; set; }
        public TimeSpan Hour { get; set; }
        public string CourseCode { get; }

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

        // Constructor: Number is now int
        public Shift(int number, ShiftType type, string day, TimeSpan hour, int capacity, string courseCode, Classroom? classroom)
        {
            Number = number;
            Type = type;
            Day = day;
            Hour = hour;
            Capacity = capacity;
            CourseCode = courseCode;
            Classroom = classroom;
            studentDAO = new StudentDAO();
            StudentKeys = new List<string>();
        }

        // Loads student keys for the shift
        public void LoadStudents()
        {
            StudentKeys.Clear();
            StudentKeys.AddRange(studentDAO.GetStudentKeysForShift(Number));  
        }

        // Add student to the shift
        public void AddStudent(string studentKey)
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
            studentDAO.EnrollStudentInShift(studentKey, CourseCode,Number); 
        }

        // Remove student from the shift
        public void RemoveStudent(string studentKey)
        {
            if (!StudentKeys.Contains(studentKey))
            {
                throw new ArgumentException("Student not found in the shift.");
            }

            StudentKeys.Remove(studentKey);
            studentDAO.UnenrollStudentFromShift(studentKey, CourseCode, Number);  
        }

        public override string ToString() =>
            $"Shift {Number}: {Type} on {Day} at {Hour} in Classroom {Classroom?.Number ?? "None"} (Capacity: {Capacity}, Students: {NumberOfStudents})";

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
