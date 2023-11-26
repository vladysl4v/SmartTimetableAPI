using System.Linq.Expressions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ServicesTests;
/*
public class SettingsServiceTests
{
    private readonly IOutagesHandler _outagesHandler;
    public SettingsServiceTests()
    {
        var dbRepositoryMock = new Mock<IDbRepository>();   
        dbRepositoryMock.Setup(x => x.Add<OutageEntity>(null!)).Throws<Exception>();
        dbRepositoryMock.Setup(x => x.Add(It.IsAny<OutageEntity>())).Returns(Task.CompletedTask);
        dbRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        dbRepositoryMock.Setup(x => x.Remove(It.IsAny<OutageEntity>()));
        dbRepositoryMock
            .Setup(x => x.Get(It.IsAny<Expression<Func<OutageEntity, bool>>>()))
            .Returns(new List<OutageEntity>().AsQueryable());
        
        _outagesHandler = new DtekOutagesHandler(dbRepositoryMock.Object);
    }
    [Fact]
    public void SettingsService_GetOutageGroups_ReturnDictionary()
    {
        // Arrange 
        var settingsService = new SettingsService(new MockHttpFactory(), _outagesHandler);
        
        // Act
        var groups = settingsService.GetOutageGroups();
        
        // Assert
        groups.Should().NotBeNull();
        groups.Should().HaveCount(6);
        groups.Should().AllSatisfy(x => x.Value.Should().StartWith("group"));
    }
    
    [Fact]
    public async Task SettingsService_GetFilters_ReturnDictionary()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":{\"__type\":\"VnzWeb.BetaSchedule+StudentScheduleFiltersData\",\"faculties\":[{\"Key\":\"467UNWISGQ6Z\",\"Value\":\"Aluminium\"},{\"Key\":\"7F0UOMJ9ATTN\",\"Value\":\"Oxygen\"},{\"Key\":\"IWTS0T0RHVHV\",\"Value\":\"Helium\"}],\"educForms\":[{\"Key\":\"1\",\"Value\":\"Light\"},{\"Key\":\"7\",\"Value\":\"Medium\"},{\"Key\":\"3\",\"Value\":\"Hard\"}],\"courses\":[{\"Key\":\"1\",\"Value\":\"1 course\"},{\"Key\":\"2\",\"Value\":\"2 course\"},{\"Key\":\"3\",\"Value\":\"3 course\"},{\"Key\":\"4\",\"Value\":\"4 course\"}]}}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudentScheduleFiltersData"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, _outagesHandler);
        
        // Act
        var filters = await settingsService.GetFilters(CancellationToken.None);
        
        // Assert
        filters.Should().NotBeNull();
        filters.Should().HaveCount(3);
        filters.Should().ContainKeys("faculties", "educForms", "courses");
        
        filters["faculties"].Should().HaveCount(3);
        filters["faculties"].Should().ContainValues("Aluminium", "Oxygen", "Helium");
        
        filters["educForms"].Should().HaveCount(3);
        filters["educForms"].Should().ContainValues("Light", "Medium", "Hard");
        
        filters["courses"].Should().HaveCount(4);
        filters["courses"].Should().AllSatisfy(x => x.Value.Should().EndWith("course"));
    }
    
    [Fact]
    public async Task SettingsService_GetFilters_ThrowsException()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":[]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudentScheduleFiltersData"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, _outagesHandler);
        
        // Act
        var act = async () => await settingsService.GetFilters(CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }

    [Fact]
    public async Task SettingsService_GetStudyGroups_ReturnDictionary()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":{\"__type\":\"VnzWeb.BetaSchedule+StudyGroupsData\",\"studyGroups\":[{\"Key\":\"89YFKCWMK592\",\"Value\":\"WTF-21\"},{\"Key\":\"6U0M6S8RJNGK\",\"Value\":\"LMAO-21\"},{\"Key\":\"3STTJCMVPQZF\",\"Value\":\"IDGAF-21\"},{\"Key\":\"6O5NH5SPYHBT\",\"Value\":\"LEET-21\"}],\"studyTypes\":null}}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudyGroups"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, _outagesHandler);

        // Act
        var studyGroups = await settingsService.GetStudyGroups("", 0, 0, CancellationToken.None);
        
        // Assert
        studyGroups.Should().NotBeNull();
        studyGroups.Should().HaveCount(4);
        studyGroups.Should().AllSatisfy(x => x.Value.Should().EndWith("-21"));
    }
    
    [Fact]
    public async Task SettingsService_GetStudyGroups_ThrowsException()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":[]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudyGroups"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, _outagesHandler);
        
        // Act
        var act = async () => await settingsService.GetStudyGroups("", 0, 0, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }
}
*/