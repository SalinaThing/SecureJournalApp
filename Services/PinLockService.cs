using System.Security.Cryptography;
using System.Text;

namespace SecureJournal.Data.Services
{
    public class PinLockService
    {
        // Prefix for storing each user's PIN hash in local Preferences
        private const string PrefPinPrefix = "SecureJournal.db.pin.hash.";

        // Tracks whether the app is currently unlocked with the user's PIN
        public bool IsUnlocked { get; private set; }

        // Current user identifier (used to save/retrieve PIN for the right user)
        public string? CurrentUser { get; set; }

        // Event triggered whenever the lock state changes (locked/unlocked)
        public event Action? LockStateChanged;

        public PinLockService()
        {
            // Start in locked state by default
            IsUnlocked = false;
        }

        // Attempt to unlock the app using the entered PIN
        public bool TryUnlock(string? enteredPin)
        {
            if (string.IsNullOrWhiteSpace(CurrentUser))
                return false;

            enteredPin = (enteredPin ?? "").Trim();

            if (enteredPin.Length == 0)
                return false;

            // Get stored PIN hash for the current user
            var savedHash = Preferences.Get($"{PrefPinPrefix}{CurrentUser}", "");
            if (string.IsNullOrWhiteSpace(savedHash))
                return false;

            // Compute hash of entered PIN
            var enteredHash = ComputeHash(enteredPin);

            // Check if entered PIN matches stored hash
            if (string.Equals(savedHash, enteredHash, StringComparison.Ordinal))
            {
                IsUnlocked = true;
                Notify();
                return true;
            }

            return false;
        }

        // Lock the app manually
        public void Lock()
        {
            IsUnlocked = false;
            Notify();
        }

        // Set a new 4-digit PIN for the current user
        public bool SetNewPin(string? newPin)
        {
            if (string.IsNullOrWhiteSpace(CurrentUser))
                return false;

            newPin = (newPin ?? "").Trim();

            // Ensure PIN is exactly 4 digits
            if (newPin.Length != 4)
                return false;

            // Ensure PIN contains only numeric digits
            if (!newPin.All(char.IsDigit))
                return false;

            // Save hashed PIN in local storage
            Preferences.Set($"{PrefPinPrefix}{CurrentUser}", ComputeHash(newPin));

            // Lock app after setting new PIN
            IsUnlocked = false;
            Notify();
            return true;
        }

        // Check if the current user has a PIN set
        public bool HasPin()
        {
            if (string.IsNullOrWhiteSpace(CurrentUser))
                return false;

            var hash = Preferences.Get($"{PrefPinPrefix}{CurrentUser}", "");
            return !string.IsNullOrWhiteSpace(hash);
        }

        // Notify subscribers that lock/unlock state has changed
        private void Notify()
        {
            LockStateChanged?.Invoke();
        }

        // Compute SHA256 hash for a given string
        private static string ComputeHash(string value)
        {
            using var sha = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(value ?? "");
            var hash = sha.ComputeHash(bytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
