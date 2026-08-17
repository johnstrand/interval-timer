using SQLite;
using IntervalTimer.Models;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IntervalTimer.Data;

public class DatabaseService
{
    SQLiteAsyncConnection Database;

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<Preset>();
        await Database.CreateTableAsync<RunHistory>();
        
        if (await Database.Table<Preset>().CountAsync() == 0)
        {
            await Database.InsertAsync(new Preset 
            { 
                Name = "Default 1m/1m", 
                RunTime = 60, 
                WalkTime = 60, 
                StartWithRun = true 
            });
        }
    }

    public async Task<List<Preset>> GetPresetsAsync()
    {
        await Init();
        return await Database.Table<Preset>().ToListAsync();
    }

    public async Task<int> SavePresetAsync(Preset item)
    {
        await Init();
        if (item.Id != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }
    
    public async Task<int> DeletePresetAsync(Preset item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }

    public async Task<List<RunHistory>> GetHistoryAsync()
    {
        await Init();
        return await Database.Table<RunHistory>().OrderByDescending(x => x.Date).ToListAsync();
    }

    public async Task<int> SaveHistoryAsync(RunHistory item)
    {
        await Init();
        return await Database.InsertAsync(item);
    }
}
