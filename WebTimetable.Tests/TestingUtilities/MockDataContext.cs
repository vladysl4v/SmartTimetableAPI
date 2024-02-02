using Microsoft.EntityFrameworkCore;
using WebTimetable.Application;

namespace WebTimetable.Tests.TestingUtilities;

public class MockDataContext : DataContext
{
    public MockDataContext() : base(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options) 
    { }
    
    public MockDataContext AddRange<T>(IEnumerable<T> items) where T : class, new()
    {
        Set<T>().AddRange(items);
        SaveChanges();
        return this;
    }
}