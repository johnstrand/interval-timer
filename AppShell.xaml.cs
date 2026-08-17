using IntervalTimer.Views;

namespace IntervalTimer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(ActiveRunPage), typeof(ActiveRunPage));
        Routing.RegisterRoute(nameof(PresetDetailPage), typeof(PresetDetailPage));
    }
}
