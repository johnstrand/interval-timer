using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntervalTimer.Data;
using IntervalTimer.Models;
using System.Threading.Tasks;
using System.Linq;

namespace IntervalTimer.ViewModels;

[QueryProperty(nameof(PresetId), "PresetId")]
public partial class PresetDetailViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int _presetId;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private int _runMinutes;

    [ObservableProperty]
    private int _runSeconds;

    [ObservableProperty]
    private int _walkMinutes;

    [ObservableProperty]
    private int _walkSeconds;

    [ObservableProperty]
    private bool _startWithRun = true;

    [ObservableProperty]
    private int _totalDurationMinutes;

    [ObservableProperty]
    private int _totalIntervals;

    public PresetDetailViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    partial void OnPresetIdChanged(int value)
    {
        if (value > 0)
        {
            LoadPresetAsync(value).ConfigureAwait(false);
        }
    }

    private async Task LoadPresetAsync(int id)
    {
        var presets = await _databaseService.GetPresetsAsync();
        var preset = presets.FirstOrDefault(p => p.Id == id);
        
        if (preset != null)
        {
            Name = preset.Name;
            RunMinutes = preset.RunTime / 60;
            RunSeconds = preset.RunTime % 60;
            WalkMinutes = preset.WalkTime / 60;
            WalkSeconds = preset.WalkTime % 60;
            StartWithRun = preset.StartWithRun;
            TotalDurationMinutes = preset.TotalDuration / 60;
            TotalIntervals = preset.TotalIntervals;
        }
    }

    [RelayCommand]
    async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a name for the preset.", "OK");
            return;
        }

        int runTime = (RunMinutes * 60) + RunSeconds;
        int walkTime = (WalkMinutes * 60) + WalkSeconds;

        if (runTime == 0 && walkTime == 0)
        {
            await Shell.Current.DisplayAlert("Error", "Run and walk times cannot both be zero.", "OK");
            return;
        }

        var preset = new Preset
        {
            Id = PresetId,
            Name = Name,
            RunTime = runTime,
            WalkTime = walkTime,
            StartWithRun = StartWithRun,
            TotalDuration = TotalDurationMinutes * 60,
            TotalIntervals = TotalIntervals
        };

        await _databaseService.SavePresetAsync(preset);
        await Shell.Current.GoToAsync("..");
    }
}
