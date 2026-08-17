using SQLite;

namespace IntervalTimer.Models;

public class Preset
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    // In seconds
    public int RunTime { get; set; }
    public int WalkTime { get; set; }
    
    public bool StartWithRun { get; set; }
    
    // Optional limits. 0 means no limit.
    public int TotalDuration { get; set; } 
    public int TotalIntervals { get; set; }
}
