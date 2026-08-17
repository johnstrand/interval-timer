using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntervalTimer.Data;
using IntervalTimer.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using Plugin.Maui.Audio;
using Microsoft.Maui.Devices;

namespace IntervalTimer.ViewModels;

[QueryProperty(nameof(PresetId), "PresetId")]
public partial class ActiveRunViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;
    private readonly IAudioManager _audioManager;
    private IDispatcherTimer _timer;

    [ObservableProperty]
    private int _presetId;

    [ObservableProperty]
    private Preset _currentPreset;

    [ObservableProperty]
    private string _currentStateText;

    [ObservableProperty]
    private string _nextStateText;

    [ObservableProperty]
    private int _remainingSeconds;

    [ObservableProperty]
    private string _remainingTimeFormatted;

    [ObservableProperty]
    private string _totalTimeFormatted;

    [ObservableProperty]
    private bool _isRunningState; // True if running, False if walking

    [ObservableProperty]
    private bool _isStarted;

    [ObservableProperty]
    private bool _isPaused;

    private int _totalElapsedSeconds = 0;
    private int _intervalsCompleted = 0;
    
    private int _actualRunSeconds = 0;
    private int _actualWalkSeconds = 0;

    public ActiveRunViewModel(DatabaseService databaseService, IAudioManager audioManager)
    {
        _databaseService = databaseService;
        _audioManager = audioManager;
    }

    partial void OnPresetIdChanged(int value)
    {
        LoadPresetAsync(value).ConfigureAwait(false);
    }

    private async Task LoadPresetAsync(int id)
    {
        var presets = await _databaseService.GetPresetsAsync();
        CurrentPreset = presets.FirstOrDefault(p => p.Id == id);
        
        if (CurrentPreset != null)
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        _totalElapsedSeconds = 0;
        _intervalsCompleted = 0;
        _actualRunSeconds = 0;
        _actualWalkSeconds = 0;
        IsRunningState = CurrentPreset.StartWithRun;
        UpdateStateDisplay();
        IsStarted = false;
        IsPaused = false;
        TotalTimeFormatted = "00:00";
    }

    private void UpdateStateDisplay()
    {
        RemainingSeconds = IsRunningState ? CurrentPreset.RunTime : CurrentPreset.WalkTime;
        CurrentStateText = IsRunningState ? "RUN" : "WALK";
        NextStateText = IsRunningState ? "Next: WALK" : "Next: RUN";
        FormatTimes();
    }

    private void FormatTimes()
    {
        RemainingTimeFormatted = $"{RemainingSeconds / 60:D2}:{RemainingSeconds % 60:D2}";
        TotalTimeFormatted = $"{_totalElapsedSeconds / 60:D2}:{_totalElapsedSeconds % 60:D2}";
    }

    [RelayCommand]
    void StartPause()
    {
        if (!IsStarted)
        {
            IsStarted = true;
            IsPaused = false;
            StartTimer();
        }
        else
        {
            IsPaused = !IsPaused;
            if (IsPaused)
            {
                _timer?.Stop();
            }
            else
            {
                _timer?.Start();
            }
        }
    }

    [RelayCommand]
    async Task Stop()
    {
        _timer?.Stop();
        
        var history = new RunHistory
        {
            Date = DateTime.Now,
            PresetName = CurrentPreset?.Name ?? "Unknown",
            TotalRunTime = _actualRunSeconds,
            TotalWalkTime = _actualWalkSeconds,
            Completed = false
        };
        await _databaseService.SaveHistoryAsync(history);

        await Shell.Current.GoToAsync("..");
    }

    private void StartTimer()
    {
        if (_timer == null)
        {
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }
        _timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        _totalElapsedSeconds++;
        RemainingSeconds--;

        if (IsRunningState) _actualRunSeconds++;
        else _actualWalkSeconds++;

        if (RemainingSeconds <= 0)
        {
            SwitchState();
        }

        FormatTimes();
    }

    private void SwitchState()
    {
        IsRunningState = !IsRunningState;
        RemainingSeconds = IsRunningState ? CurrentPreset.RunTime : CurrentPreset.WalkTime;
        CurrentStateText = IsRunningState ? "RUN" : "WALK";
        NextStateText = IsRunningState ? "Next: WALK" : "Next: RUN";
        
        _intervalsCompleted++;

        PlayAlert();
        
        if (CurrentPreset.TotalDuration > 0 && _totalElapsedSeconds >= CurrentPreset.TotalDuration)
        {
            FinishRun();
        }
        else if (CurrentPreset.TotalIntervals > 0 && _intervalsCompleted >= CurrentPreset.TotalIntervals)
        {
            FinishRun();
        }
    }

    private void PlayAlert()
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromSeconds(1));
        }
        catch (Exception) { /* Ignored if unsupported */ }
    }

    private async void FinishRun()
    {
        _timer?.Stop();
        var history = new RunHistory
        {
            Date = DateTime.Now,
            PresetName = CurrentPreset?.Name ?? "Unknown",
            TotalRunTime = _actualRunSeconds,
            TotalWalkTime = _actualWalkSeconds,
            Completed = true
        };
        await _databaseService.SaveHistoryAsync(history);
        await Shell.Current.GoToAsync("..");
    }
}
