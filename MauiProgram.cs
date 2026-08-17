using Microsoft.Extensions.Logging;
using IntervalTimer.Data;
using IntervalTimer.ViewModels;
using IntervalTimer.Views;
using Plugin.Maui.Audio;

namespace IntervalTimer;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Add Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton(AudioManager.Current);

        // Add ViewModels
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<PresetsViewModel>();
        builder.Services.AddTransient<HistoryViewModel>();
        builder.Services.AddTransient<ActiveRunViewModel>();
        builder.Services.AddTransient<PresetDetailViewModel>();

        // Add Views
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<PresetsPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<ActiveRunPage>();
        builder.Services.AddTransient<PresetDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
