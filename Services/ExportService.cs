using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SecureJournal.Data.Models;

namespace SecureJournal.Data.Services
{
    public class ExportService
    {
        // Service used to fetch journal entries
        private readonly JournalEntryService _entries;

        public ExportService(JournalEntryService entries)
        {
            _entries = entries;

            // Set QuestPDF license type (required by the library)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        /// <summary>
        /// Export journal entries within a specific date range to a PDF file.
        /// Returns the full path of the generated PDF.
        /// </summary>
        public async Task<string> ExportRangeAsync(DateTime from, DateTime to)
        {
            // Fetch entries from the JournalEntryService
            var list = await _entries.GetRangeAsync(from, to);

            // Ensure "Exports" folder exists in app data directory
            var folder = Path.Combine(FileSystem.AppDataDirectory, "Exports");
            Directory.CreateDirectory(folder);

            // Generate a unique filename using date range and timestamp
            var fileName = $"Journal_{from:yyyyMMdd}_{to:yyyyMMdd}_{DateTime.Now:HHmmss}.pdf";
            var path = Path.Combine(folder, fileName);

            // Generate the PDF at the specified path
            ExportToPath(list, path);
            return path;
        }

        /// <summary>
        /// Generate a PDF from the provided list of journal entries and save it to filePath.
        /// </summary>
        private static void ExportToPath(List<JournalEntry> entries, string filePath)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Page setup
                    page.Margin(25);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    // Header of the PDF
                    page.Header().Text("SecureJournal Export").FontSize(18).SemiBold();

                    // Main content section
                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        if (entries.Count == 0)
                        {
                            col.Item().Text("No entries found for this date range.");
                            return;
                        }

                        // Loop through each journal entry
                        foreach (var e in entries.OrderBy(x => x.EntryDate))
                        {
                            // Each entry displayed as a bordered card
                            col.Item().Border(1).Padding(10).Column(card =>
                            {
                                card.Spacing(4);

                                // Entry title with date
                                card.Item().Text($"{e.EntryDate:yyyy-MM-dd}  •  {e.Title}").SemiBold();

                                // Category and moods
                                card.Item().Text($"Category: {e.Category}");
                                card.Item().Text($"Mood: {e.PrimaryMood}"
                                    + (string.IsNullOrWhiteSpace(e.SecondaryMood1) ? "" : $", {e.SecondaryMood1}")
                                    + (string.IsNullOrWhiteSpace(e.SecondaryMood2) ? "" : $", {e.SecondaryMood2}")
                                );

                                // Display tags if available
                                if (!string.IsNullOrWhiteSpace(e.TagsCsv))
                                    card.Item().Text($"Tags: {e.TagsCsv}");

                                // Horizontal line to separate header from content
                                card.Item().LineHorizontal(1);

                                // Export the main content of the journal
                                var body = (e.Markdown ?? "").Trim();
                                if (body.Length == 0) body = "(empty)";

                                card.Item().Text(body);
                            });
                        }
                    });

                    // Footer with generation timestamp
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated: ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
                });
            })
            .GeneratePdf(filePath); // Save PDF to disk
        }
    }
}
