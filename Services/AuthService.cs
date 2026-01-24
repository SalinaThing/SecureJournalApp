using SecureJournalApp.Models;

namespace SecureJournalApp.Services
{
    public class AuthService
    {
        private readonly User _user = new() { Id = 1, Pin = "1234" };
        public bool IsLoggedIn { get; private set; } = false;

        public bool Login(string pin)
        {
            if (pin == _user.Pin)
            {
                IsLoggedIn = true;
                return true;
            }
            return false;
        }

        public void Logout() => IsLoggedIn = false;
    }
}
