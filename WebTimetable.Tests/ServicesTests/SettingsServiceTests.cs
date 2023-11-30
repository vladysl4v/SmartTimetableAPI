using System.Linq.Expressions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ServicesTests;


public class SettingsServiceTests
{
    [Fact]
    public async Task SettingsService_GetFilters_ReturnDictionary()
    {
        // Arrange 
        var mockDbRepository = new Mock<IDbRepository>();
        mockDbRepository.Setup(x => x.Get(It.IsAny<Expression<Func<OutageEntity, bool>>>()))
            .Returns(new List<OutageEntity>()
                {
                    new() { Group = "Group 1" }, 
                    new() { Group = "Group 1" }, 
                    new() { Group = "Group 2" }, 
                    new() { Group = "Group 2" }, 
                    new() { Group = "Group 3" }, 
                    new() { Group = "Group 3" }
                }.AsQueryable());
        
        const string mockFiltersData = "{\"d\":{\"__type\":\"VnzWeb.BetaSchedule+StudentScheduleFiltersData\",\"faculties\":[{\"Key\":\"467UNWISGQ6Z\",\"Value\":\"Aluminium\"},{\"Key\":\"7F0UOMJ9ATTN\",\"Value\":\"Oxygen\"},{\"Key\":\"IWTS0T0RHVHV\",\"Value\":\"Helium\"}],\"educForms\":[{\"Key\":\"1\",\"Value\":\"Light\"},{\"Key\":\"7\",\"Value\":\"Medium\"},{\"Key\":\"3\",\"Value\":\"Hard\"}],\"courses\":[{\"Key\":\"1\",\"Value\":\"1 course\"},{\"Key\":\"2\",\"Value\":\"2 course\"},{\"Key\":\"3\",\"Value\":\"3 course\"},{\"Key\":\"4\",\"Value\":\"4 course\"}]}}";
        
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudentScheduleFiltersData"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, mockDbRepository.Object);
        
        // Act
        var filters = await settingsService.GetFilters(CancellationToken.None);
        
        // Assert
        filters.Should().NotBeNull();
        filters.Should().HaveCount(4);
        filters.Should().ContainKeys("faculties", "educForms", "courses", "outageGroups");
        
        filters["faculties"].Should().HaveCount(3);
        filters["faculties"].Should().ContainValues("Aluminium", "Oxygen", "Helium");
        
        filters["educForms"].Should().HaveCount(3);
        filters["educForms"].Should().ContainValues("Light", "Medium", "Hard");
        
        filters["courses"].Should().HaveCount(4);
        filters["courses"].Should().AllSatisfy(x => x.Value.Should().EndWith("course"));
        
        filters["outageGroups"].Should().HaveCount(3);
        filters["outageGroups"].Should().AllSatisfy(x => x.Value.Should().StartWith("Group"));
    }
    
    [Fact]
    public async Task SettingsService_GetFilters_ThrowsException()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":[]}";
        var mockDbRepository = new Mock<IDbRepository>();
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudentScheduleFiltersData"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, mockDbRepository.Object);
        
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
        var mockDbRepository = new Mock<IDbRepository>();
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudyGroups"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, mockDbRepository.Object);

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
        var mockDbRepository = new Mock<IDbRepository>();
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudyGroups"), mockFiltersData);
        var settingsService = new SettingsService(mockHttpFactory, mockDbRepository.Object);
        
        // Act
        var act = async () => await settingsService.GetStudyGroups("", 0, 0, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }
}