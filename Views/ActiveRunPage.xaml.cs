using IntervalTimer.ViewModels;

namespace IntervalTimer.Views;

public partial class ActiveRunPage : ContentPage
{
    public ActiveRunPage(ActiveRunViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
