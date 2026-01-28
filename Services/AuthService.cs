using System.Security.Cryptography;
using System.Text;

namespace SecureJournal.Data.Services
{
    public class AuthService
    {
        // Key for storing current logged-in user
        private const string PrefCurrentUser = "auth.current.user";

        // Prefix for storing individual user passwords
        private const string PrefUserPrefix = "auth.user.";

        // Key for checking if PIN setup is complete for the user
        private const string PrefPinSetup = "auth.pin.setup";

        // Currently logged-in username
        public string? CurrentUser { get; private set; }

        // Indicates if the current user has completed PIN setup
        public bool IsPinSetup { get; private set; }

        // Event triggered whenever authentication state changes
        public event Action? AuthStateChanged;

        public AuthService()
        {
            // Load current user and PIN setup status from preferences
            CurrentUser = Preferences.Get(PrefCurrentUser, null);
            IsPinSetup = Preferences.Get(PrefPinSetup, false);
        }

        // Registers a new user with username and password
        public bool Register(string username, string password)
        {
            username = (username ?? "").Trim();
            password = (password ?? "").Trim();

            if (username.Length < 3) return false;
            if (password.Length < 4) return false;

            // Check if user already exists
            var existingHash = Preferences.Get($"{PrefUserPrefix}{username}", "");
            if (!string.IsNullOrWhiteSpace(existingHash)) return false;

            // Hash the password and store
            var hash = ComputeHash(password);
            Preferences.Set($"{PrefUserPrefix}{username}", hash);

            // Set current user and reset PIN setup
            CurrentUser = username;
            Preferences.Set(PrefCurrentUser, username);
            IsPinSetup = false;
            Preferences.Set(PrefPinSetup, false);

            Notify();
            return true;
        }

        // Logs in an existing user by verifying password
        public bool Login(string username, string password)
        {
            username = (username ?? "").Trim();
            password = (password ?? "").Trim();

            if (username.Length == 0 || password.Length == 0) return false;

            // Retrieve stored password hash
            var savedHash = Preferences.Get($"{PrefUserPrefix}{username}", "");
            if (string.IsNullOrWhiteSpace(savedHash)) return false;

            // Compare entered password hash
            var enteredHash = ComputeHash(password);
            if (!string.Equals(savedHash, enteredHash, StringComparison.Ordinal)) return false;

            // Set current user and load PIN setup status
            CurrentUser = username;
            Preferences.Set(PrefCurrentUser, username);
            IsPinSetup = Preferences.Get($"{PrefPinSetup}{username}", false);

            Notify();
            return true;
        }

        // Logs out the current user and clears related preferences
        public void Logout()
        {
            CurrentUser = null;
            IsPinSetup = false;
            Preferences.Remove(PrefCurrentUser);
            Preferences.Remove(PrefPinSetup);

            Notify();
        }

        // Marks that the current user has completed PIN setup
        public bool SetPinSetupComplete()
        {
            if (string.IsNullOrWhiteSpace(CurrentUser)) return false;

            IsPinSetup = true;
            Preferences.Set(PrefPinSetup, true);

            Notify();
            return true;
        }

        // Notify subscribers about authentication state changes
        private void Notify()
        {
            AuthStateChanged?.Invoke();
        }

        // Compute SHA256 hash for a string (used for password storage)
        private static string ComputeHash(string value)
        {
            using var sa = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(value ?? "");
            var hash = sa.ComputeHash(bytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
