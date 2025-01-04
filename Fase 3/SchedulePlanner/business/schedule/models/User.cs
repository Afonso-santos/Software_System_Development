namespace SchedulePlanner.business.schedule.models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        public string Username { get; }
        public string Password { get; private set; }
        public bool   Admin    { get; }

        /// <summary>
        /// Constructs a User object.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="admin">Whether the user is an admin.</param>
        public User(string username, string password, bool admin)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Admin    = admin;
        }

        /// <summary>
        /// Updates the password for the user.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        public void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password cannot be empty.", nameof(newPassword));

            Password = newPassword;
        }

        /// <summary>
        /// Returns a string representation of the User object.
        /// </summary>
        public override string ToString() => Username;

        /// <summary>
        /// Checks if two User objects are equal.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var user = (User)obj;
            return Username == user.Username;
        }

        public override int GetHashCode() => Username.GetHashCode();

        public static bool operator ==(User? a, User? b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            return a.Username == b.Username;
        }

        public static bool operator !=(User? a, User? b) => !(a == b);
    }
}
