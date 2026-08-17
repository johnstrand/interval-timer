using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntervalTimer.Data;
using IntervalTimer.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IntervalTimer.ViewModels;

public partial class PresetsViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<Preset> _presets = new();

    public PresetsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task LoadPresetsAsync()
    {
        var items = await _databaseService.GetPresetsAsync();
        Presets.Clear();
        foreach (var item in items)
        {
            Presets.Add(item);
        }
    }

    [RelayCommand]
    async Task AddNewPreset()
    {
        await Shell.Current.GoToAsync("PresetDetailPage");
    }

    [RelayCommand]
    async Task StartPreset(Preset preset)
    {
        if (preset == null) return;
        await Shell.Current.GoToAsync($"ActiveRunPage?PresetId={preset.Id}");
    }

    [RelayCommand]
    async Task EditPreset(Preset preset)
    {
        if (preset == null) return;
        await Shell.Current.GoToAsync($"PresetDetailPage?PresetId={preset.Id}");
    }
    
    [RelayCommand]
    async Task DeletePreset(Preset preset)
    {
        if (preset == null) return;
        bool confirm = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete '{preset.Name}'?", "Yes", "No");
        if (confirm)
        {
            await _databaseService.DeletePresetAsync(preset);
            await LoadPresetsAsync();
        }
    }
}
