using Microsoft.EntityFrameworkCore;
using WebTimetable.Application;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;

namespace WebTimetable.Tests.ApplicationUnitTests.Repositories;

public class DbRepositoryTests
{
    [Fact]
    public async Task DbRepository_GetAll_ReturnsQueryable()
    {
        // Arrange
        var dbRepository = await GetMockDbRepository(GetMockOutages());
        
        // Act
        var result = dbRepository.GetAll<OutageEntity>();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task DbRepository_Get_ReturnsQueryable()
    {
        // Arrange
        var dbRepository = await GetMockDbRepository(GetMockOutages());
        
        // Act
        var result = dbRepository.Get<OutageEntity>(x => x.DayOfWeek == DayOfWeek.Monday);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Single().DayOfWeek.Should().Be(DayOfWeek.Monday);
    }
    
    [Fact]
    public async Task DbRepository_Get_ReturnsEmptyQueryable()
    {
        // Arrange
        var dbRepository = await GetMockDbRepository(GetMockOutages());
        
        // Act
        var result = dbRepository.Get<OutageEntity>(x => x.DayOfWeek == DayOfWeek.Thursday);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task DbRepository_Add_Successfully()
    {
        // Arrange
        var dbRepository = await GetMockDbRepository(new List<OutageEntity>());
        
        // Act
        await dbRepository.Add(new OutageEntity { DayOfWeek = DayOfWeek.Thursday, City = "Test city", Group = "Test group", Outages = new List<Outage>() });
        await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        dbRepository.GetAll<OutageEntity>().Should().HaveCount(1);
        dbRepository.GetAll<OutageEntity>().Single().DayOfWeek.Should().Be(DayOfWeek.Thursday);
    }
    
    [Fact]
    public async Task DbRepository_AddRange_Successfully()
    {
        // Arrange
        var dbRepository = await GetMockDbRepository(new List<OutageEntity>());
        var seedData = GetMockOutages();
        
        // Act
        await dbRepository.AddRange(seedData);
        await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        dbRepository.GetAll<OutageEntity>().Should().HaveCount(2);
        dbRepository.GetAll<OutageEntity>().Should().BeEquivalentTo(seedData);
    }
    
    [Fact]
    public async Task DbRepository_FindAsync_ReturnsOutageEntity()
    {
        // Arrange
        var dbRepository = await GetMockDbRepository(GetMockOutages());
        
        // Act
        var result = await dbRepository.FindAsync<OutageEntity>("Test city", "Test group", DayOfWeek.Monday);
        
        // Assert
        result.Should().NotBeNull();
        result!.City.Should().Be("Test city");
        result!.Group.Should().Be("Test group");
        result!.DayOfWeek.Should().Be(DayOfWeek.Monday);
        result!.Outages.Should().NotBeEmpty();
    }
    
    [Theory]
    [InlineData("Test city", "Test group", DayOfWeek.Tuesday)]
    [InlineData("Random city", "Test group", DayOfWeek.Monday)]
    [InlineData("Test city", "Random group", DayOfWeek.Monday)]
    public async Task DbRepository_FindAsync_ReturnsNull(string city, string group, DayOfWeek dayOfWeek)
    {
        // Arrange
        var dbRepository = await GetMockDbRepository(GetMockOutages());
        
        // Act
        var result = await dbRepository.FindAsync<OutageEntity>(city, group, dayOfWeek);
        
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task DbRepository_Remove_Successfully()
    {
        // Arrange
        var seedData = GetMockOutages();
        var toDelete = seedData.First();
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.Remove(toDelete);
        await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        dbRepository.GetAll<OutageEntity>().Should().NotContain(toDelete);
    }
    
    [Fact]
    public async Task DbRepository_Remove_Unsuccessfully()
    {
        // Arrange
        var seedData = GetMockOutages();
        var toDelete = new OutageEntity
        {
            City = "Random city",
            Group = "Random group",
            DayOfWeek = DayOfWeek.Monday,
            Outages = new List<Outage>()
        };
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.Remove(toDelete);
        var act = async () => await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
        dbRepository.GetAll<OutageEntity>().Should().BeEquivalentTo(seedData);
    }
    
    [Fact]
    public async Task DbRepository_RemoveRange_Successfully()
    {
        // Arrange
        var seedData = GetMockOutages();
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.RemoveRange(seedData);
        await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        dbRepository.GetAll<OutageEntity>().Should().BeEmpty();
    }
    
    [Fact]
    public async Task DbRepository_RemoveRange_ShouldThrowException()
    {
        // Arrange
        var seedData = GetMockOutages();
        var toDelete = new List<OutageEntity>
        {
            new()
            {
                City = "Random city",
                Group = "Random group",
                DayOfWeek = DayOfWeek.Monday,
                Outages = new List<Outage>()
            }
        };
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.RemoveRange(toDelete);
        var act = async () => await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
        dbRepository.GetAll<OutageEntity>().Should().BeEquivalentTo(seedData);
    }
    
        [Fact]
    public async Task DbRepository_Update_Successfully()
    {
        // Arrange
        var seedData = GetMockOutages();
        var toUpdate = seedData.First();
        var beforeUpdate = new OutageEntity { City = toUpdate.City, Group = toUpdate.Group, DayOfWeek = toUpdate.DayOfWeek, Outages = toUpdate.Outages };
        toUpdate.Outages = new List<Outage>(new[] { new Outage(), new Outage(), new Outage() });
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.Update(toUpdate);
        await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        dbRepository.GetAll<OutageEntity>().Should().NotContain(beforeUpdate);
        dbRepository.GetAll<OutageEntity>().Should().Contain(toUpdate);
    }
    
    [Fact]
    public async Task DbRepository_Update_ShouldThrowException()
    {
        // Arrange
        var seedData = GetMockOutages();
        var tryToUpdate = new OutageEntity
        {
            City = "Random city",
            Group = "Random group",
            DayOfWeek = DayOfWeek.Monday,
            Outages = new List<Outage>()
        };
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.Update(tryToUpdate);
        var act = async () => await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
        dbRepository.GetAll<OutageEntity>().Should().BeEquivalentTo(seedData);
    }
    
    [Fact]
    public async Task DbRepository_UpdateRange_Successfully()
    {
        // Arrange
        var seedData = GetMockOutages();
        var toUpdate = seedData.First();
        var beforeUpdate = new OutageEntity { City = toUpdate.City, Group = toUpdate.Group, DayOfWeek = toUpdate.DayOfWeek, Outages = toUpdate.Outages };
        toUpdate.Outages = new List<Outage>(new[] { new Outage(), new Outage(), new Outage() });
        var toUpdateList = new List<OutageEntity> { toUpdate };
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.UpdateRange(toUpdateList);
        await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        dbRepository.GetAll<OutageEntity>().Should().NotContain(beforeUpdate);
        dbRepository.GetAll<OutageEntity>().Should().Contain(toUpdate);
    }
    
    [Fact]
    public async Task DbRepository_UpdateRange_ShouldThrowException()
    {
        // Arrange
        var seedData = GetMockOutages();
        var tryToUpdate = new OutageEntity
        {
            City = "Random city",
            Group = "Random group",
            DayOfWeek = DayOfWeek.Monday,
            Outages = new List<Outage>()
        };
        var tryToUpdateList = new List<OutageEntity> { tryToUpdate };
        var dbRepository = await GetMockDbRepository(seedData);
        
        // Act
        dbRepository.UpdateRange(tryToUpdateList);
        var act = async () => await dbRepository.SaveChangesAsync(CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
        dbRepository.GetAll<OutageEntity>().Should().BeEquivalentTo(seedData);
    }
    
    private async Task<DbRepository> GetMockDbRepository<T>(IEnumerable<T> sourceList) where T : class
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        var dbContext = new DataContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        
        if (!await dbContext.Set<T>().AnyAsync())
        {
            await dbContext.Set<T>().AddRangeAsync(sourceList);
            await dbContext.SaveChangesAsync();
        }

        return new DbRepository(dbContext);
    }

    private List<OutageEntity> GetMockOutages()
    {
        return new List<OutageEntity>
        {
            new()
            {
                City = "Test city",
                Group = "Test group",
                DayOfWeek = DayOfWeek.Monday,
                Outages = new List<Outage>
                {
                    new()
                }
            },
            new()
            {
                City = "2 Test city",
                Group = "2 Test group",
                DayOfWeek = DayOfWeek.Tuesday,
                Outages = new List<Outage>()
            }
        };
    }
}