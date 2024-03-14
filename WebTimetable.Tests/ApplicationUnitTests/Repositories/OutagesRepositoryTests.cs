using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ApplicationUnitTests.Repositories;

public class OutagesRepositoryTests
{
    private readonly MockDataContext _mockDataContext = new();

    [Fact]
    public void OutagesRepository_GetOutageGroups_ReturnsOutages()
    {
        // Arrange
        _mockDataContext.AddRange(new List<OutageEntity>
            {
                new() { Group = "Group 1", City = "Kyiv", DayOfWeek = DayOfWeek.Monday, Outages = new List<Outage>() }, 
                new() { Group = "Group 1", City = "Kyiv", DayOfWeek = DayOfWeek.Tuesday, Outages = new List<Outage>() }, 
                new() { Group = "Group 2", City = "Kyiv", DayOfWeek = DayOfWeek.Monday, Outages = new List<Outage>() }, 
                new() { Group = "Group 2", City = "Kyiv", DayOfWeek = DayOfWeek.Tuesday, Outages = new List<Outage>() }, 
                new() { Group = "Group 3", City = "Kyiv", DayOfWeek = DayOfWeek.Monday, Outages = new List<Outage>() }, 
                new() { Group = "Group 3", City = "Kyiv", DayOfWeek = DayOfWeek.Tuesday, Outages = new List<Outage>() }
            });
        
        var outagesRepository = new OutagesRepository(_mockDataContext);

        // Act
        var outages = outagesRepository.GetOutageGroups("Kyiv");

        // Assert
        outages.Should().NotBeNull();
        outages.Should().HaveCount(3);
        outages.Should().AllSatisfy(x => x.Value.Should().StartWith("Group"));
        outages.Should().AllSatisfy(x => x.Key.Should().Be(x.Value));
    }
    
    [Fact]
    public void OutagesRepository_GetOutageGroups_ReturnsNothing()
    {
        // Arrange
        _mockDataContext.AddRange(new List<OutageEntity>
        {
            new() { Group = "Group 1", City = "Kyiv", DayOfWeek = DayOfWeek.Monday, Outages = new List<Outage>() }, 
            new() { Group = "Group 1", City = "Kyiv", DayOfWeek = DayOfWeek.Tuesday, Outages = new List<Outage>() }, 
            new() { Group = "Group 2", City = "Kyiv", DayOfWeek = DayOfWeek.Monday, Outages = new List<Outage>() }, 
            new() { Group = "Group 2", City = "Kyiv", DayOfWeek = DayOfWeek.Tuesday, Outages = new List<Outage>() }, 
            new() { Group = "Group 3", City = "Kyiv", DayOfWeek = DayOfWeek.Monday, Outages = new List<Outage>() }, 
            new() { Group = "Group 3", City = "Kyiv", DayOfWeek = DayOfWeek.Tuesday, Outages = new List<Outage>() }
        });
        
        var outagesRepository = new OutagesRepository(_mockDataContext);

        // Act
        var outages = outagesRepository.GetOutageGroups("Dnipro");

        // Assert
        outages.Should().NotBeNull();
        outages.Should().BeEmpty();
    }

    [Fact]
    public async Task OutagesRepository_GetOutagesByDayOfWeekAsync_ReturnOutages()
    {
        // Arrange
        _mockDataContext.AddRange(new OutageEntity
        {
            Group = "1st",
            City = "Kyiv",
            DayOfWeek = DayOfWeek.Monday,
            Outages = new List<Outage>
            {
                new() { Start = new TimeOnly(11, 00), End = new TimeOnly(12, 00) },
            }
        },
        new OutageEntity
        {
            Group = "2nd",
            City = "Kyiv",
            DayOfWeek = DayOfWeek.Monday,
            Outages = new List<Outage>
            {
                new() { Start = new TimeOnly(13, 00), End = new TimeOnly(14, 00) },
                new() { Start = new TimeOnly(15, 00), End = new TimeOnly(16, 00) }
            },
        });
        
        // Act
        var outagesRepository = new OutagesRepository(_mockDataContext);
        var outages = await outagesRepository.GetOutagesByDayOfWeekAsync(DayOfWeek.Monday, "1st", CancellationToken.None);
        
        // Assert
        outages.Should().NotBeNull();
        outages.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task OutagesRepository_GetOutagesByDayOfWeekAsync_ReturnNothing()
    {
        // Arrange
        _mockDataContext.AddRange(new OutageEntity
            {
                Group = "1st",
                City = "Kyiv",
                DayOfWeek = DayOfWeek.Monday,
                Outages = new List<Outage>
                {
                    new() { Start = new TimeOnly(11, 00), End = new TimeOnly(12, 00) },
                }
            });
        
        // Act
        var outagesRepository = new OutagesRepository(_mockDataContext);
        var outages = await outagesRepository.GetOutagesByDayOfWeekAsync(DayOfWeek.Monday, "2nd", CancellationToken.None);
        
        // Assert
        outages.Should().BeNull();
    }
    
    [Fact]
    public async Task OutagesRepository_AddOutagesAsync_AddsOutages()
    {
        // Arrange
        var oldOutages = new OutageEntity
        {
            Group = "3rd",
            City = "Kyiv",
            DayOfWeek = DayOfWeek.Tuesday,
            Outages = new List<Outage>
            {
                new() { Start = new TimeOnly(11, 00), End = new TimeOnly(12, 00) },
            }
        };
        _mockDataContext.Set<OutageEntity>().Add(oldOutages);
        await _mockDataContext.SaveChangesAsync(CancellationToken.None);
        var outagesRepository = new OutagesRepository(_mockDataContext);
        
        var outageGroups = new Dictionary<string, string> { { "1", "Group 1" } };
        var outages = new Dictionary<int, Dictionary<DayOfWeek, List<Outage>>>()
        {
            {
                1, new Dictionary<DayOfWeek, List<Outage>>()
                {
                    {
                        DayOfWeek.Monday, new List<Outage>()
                        {
                            new() { Start = new TimeOnly(11, 00), End = new TimeOnly(12, 00) },
                            new() { Start = new TimeOnly(13, 00), End = new TimeOnly(14, 00) },
                            new() { Start = new TimeOnly(15, 00), End = new TimeOnly(16, 00) }
                        }
                    }
                }
            }
        };
        
        // Act
        await outagesRepository.AddOutagesAsync(outages, outageGroups, CancellationToken.None);
        
        // Assert
        _mockDataContext.Set<OutageEntity>().Should().HaveCount(1);
        _mockDataContext.Set<OutageEntity>().Single().Outages.Should().HaveCount(3);
        _mockDataContext.Set<OutageEntity>().Single().City.Should().Be("Kyiv");
        _mockDataContext.Set<OutageEntity>().Single().DayOfWeek.Should().Be(DayOfWeek.Monday);
        _mockDataContext.Set<OutageEntity>().Single().Group.Should().Be(outageGroups.First().Value);
    }
    
}