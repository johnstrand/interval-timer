using IntervalTimer.ViewModels;

namespace IntervalTimer.Views;

public partial class PresetDetailPage : ContentPage
{
    public PresetDetailPage(PresetDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
