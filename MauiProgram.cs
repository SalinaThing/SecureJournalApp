using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SecureJournalApp.Services; // <-- AuthService namespace

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

            // AuthService
            builder.Services.AddSingleton<JournalService>();
            builder.Services.AddSingleton<AuthService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
