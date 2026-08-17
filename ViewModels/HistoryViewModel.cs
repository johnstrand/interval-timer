using CommunityToolkit.Mvvm.ComponentModel;
using IntervalTimer.Data;
using IntervalTimer.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace IntervalTimer.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<RunHistory> _historyItems = new();

    public HistoryViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task LoadHistoryAsync()
    {
        var items = await _databaseService.GetHistoryAsync();
        HistoryItems.Clear();
        foreach (var item in items)
        {
            HistoryItems.Add(item);
        }
    }
}
