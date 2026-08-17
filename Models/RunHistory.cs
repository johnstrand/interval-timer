using SQLite;
using System;

namespace IntervalTimer.Models;

public class RunHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public string PresetName { get; set; } = string.Empty;
    
    // In seconds
    public int TotalRunTime { get; set; }
    public int TotalWalkTime { get; set; }
    
    public bool Completed { get; set; }
}
