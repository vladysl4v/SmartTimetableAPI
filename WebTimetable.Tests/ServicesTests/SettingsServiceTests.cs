using FluentAssertions;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Handlers.Outages;
using WebTimetable.Application.Services;
using WebTimetable.Tests.TestingUtilities;
using Xunit;

namespace WebTimetable.Tests.ServicesTests;

public class SettingsServiceTests
{
    private readonly IOutagesHandler _outagesHandler;
    public SettingsServiceTests()
    {
        var mockOutageData = "\"data\":{\"1\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}},\"2\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}},\"3\":{\"1\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"2\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"3\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"4\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"5\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"6\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"7\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"}},\"4\":{\"1\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"2\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"3\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"4\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"5\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"6\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"7\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"}},\"5\":{\"1\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"2\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"3\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"4\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"5\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"6\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"7\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"}},\"6\":{\"1\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"2\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"3\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"4\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"5\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"6\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"7\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"}}}}\n\r\n\"sch_names\":{\"1\":\"group 1\",\"2\":\"group 2\",\"3\":\"group 3\",\"4\":\"group 4\",\"5\":\"group 5\",\"6\":\"group 6\"}";
        var outageHttpFactory = new MockHttpFactory().Setup(mockOutageData);
        _outagesHandler = new DtekOutagesHandler(outageHttpFactory);
        _outagesHandler.InitializeOutages().GetAwaiter().GetResult();
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