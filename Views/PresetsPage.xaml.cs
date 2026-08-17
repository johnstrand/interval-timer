using IntervalTimer.ViewModels;

namespace IntervalTimer.Views;

public partial class PresetsPage : ContentPage
{
    private readonly PresetsViewModel _viewModel;

    public PresetsPage(PresetsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadPresetsAsync();
    }
}
