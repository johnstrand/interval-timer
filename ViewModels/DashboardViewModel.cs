using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntervalTimer.Data;
using IntervalTimer.Models;
using System.Threading.Tasks;
using System.Linq;

namespace IntervalTimer.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private Preset _lastPreset;

    [ObservableProperty]
    private bool _hasPreset;

    public DashboardViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task LoadDataAsync()
    {
        var presets = await _databaseService.GetPresetsAsync();
        LastPreset = presets.FirstOrDefault();
        HasPreset = LastPreset != null;
    }

    [RelayCommand]
    async Task StartRun()
    {
        if (LastPreset != null)
        {
            await Shell.Current.GoToAsync($"ActiveRunPage?PresetId={LastPreset.Id}");
        }
    }
}
