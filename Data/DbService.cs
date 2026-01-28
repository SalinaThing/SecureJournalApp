using SQLite;

namespace SecureJournal.Data
{
    public class DbService
    {
        // Internal SQLite async connection instance
        private SQLiteAsyncConnection? _conn;

        // Task to ensure DB is initialized only once
        private Task? _initTask;

        // Public accessor for the SQLite connection
        // Throws exception if accessed before initialization
        public SQLiteAsyncConnection Connection
        {
            get
            {
                if (_conn == null) throw new InvalidOperationException("DB not initialized yet.");
                return _conn;
            }
        }

        // Ensure the database is ready before use
        // Initializes the connection and tables if not already done
        public Task EnsureReadyAsync()
        {
            // Only initialize once
            _initTask ??= InitAsync();
            return _initTask;
        }

        // Internal method to initialize the database
        private async Task InitAsync()
        {
            // Define path to database file
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "SecureJournal.db");

            // Create SQLite async connection with read/write, create, and shared cache flags
            _conn = new SQLiteAsyncConnection(
                dbPath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache
            );

            // Ensure the JournalEntry table exists
            await _conn.CreateTableAsync<Models.JournalEntry>();
        }
    }
}
