namespace SecureJournalApp.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public string PrimaryMood { get; set; } = string.Empty;
        public List<string> SecondaryMoods { get; set; } = new();

        public List<string> Tags { get; set; } = new();
    }
}
