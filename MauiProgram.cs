using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SecureJournal.Data;
using SecureJournal.Data.Services;

namespace SecureJournalApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Blazor & MudBlazor
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            // Database + services
            builder.Services.AddSingleton<DbService>();            
            builder.Services.AddSingleton<JournalEntryService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<CalendarService>();
            builder.Services.AddSingleton<PinLockService>();
            builder.Services.AddSingleton<ExportService>();


            return builder.Build();
        }
    }
}
