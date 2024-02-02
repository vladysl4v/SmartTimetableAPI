using Microsoft.EntityFrameworkCore;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories.Abstractions;

namespace WebTimetable.Application.Repositories;

public class OutagesRepository : IOutagesRepository
{
    private readonly DataContext _context;

    public OutagesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Outage>?> GetOutagesByDayOfWeekAsync(DayOfWeek dayOfWeek, string outageGroup, CancellationToken token)
    {
        var outagesEntity = await _context.Outages.FindAsync(new object?[] { "Kyiv", outageGroup, dayOfWeek },
            cancellationToken: token);
        if (outagesEntity is null)
        {
            return null;
        }
        _context.Entry(outagesEntity).State = EntityState.Detached;
        return outagesEntity.Outages;
    }

    public async Task AddOutagesAsync(Dictionary<int, Dictionary<DayOfWeek, List<Outage>>> outages,
        Dictionary<string, string> outageGroups, CancellationToken token)
    {
        try
        {
            await _context.Outages.ExecuteDeleteAsync(cancellationToken: token);
        }
        catch (InvalidOperationException)
        {
            _context.Outages.RemoveRange(_context.Outages);
            await _context.SaveChangesAsync(token);
        }
        foreach (var group in outages)
        {
            foreach (var dayOfWeek in group.Value)
            {
                await _context.Outages.AddAsync(new OutageEntity
                {
                    City = "Kyiv",
                    Group = outageGroups[group.Key.ToString()],
                    DayOfWeek = dayOfWeek.Key,
                    Outages = dayOfWeek.Value
                }, token);
            }
        }
        await _context.SaveChangesAsync(token);
    }
    
    public List<KeyValuePair<string, string>> GetOutageGroups()
    {
        return _context.Outages.AsNoTracking().Where(x => x.City == "Kyiv").Select(y => y.Group)
            .Distinct().ToDictionary(key => key, value => value).ToList();
    }
}