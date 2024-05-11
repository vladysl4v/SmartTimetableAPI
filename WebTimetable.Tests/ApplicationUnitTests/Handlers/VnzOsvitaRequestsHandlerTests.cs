using FluentAssertions.Extensions;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Handlers;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ApplicationUnitTests.Handlers;

public class VnzOsvitaRequestsHandlerTests
{
    #region GetStudentSchedule
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetStudentSchedule_ReturnStudentSchedule()
    {
        // Arrange
        var mockResponse = "{\"d\":[{\"__type\":\"VnzWeb.WidgetSchedule+ScheduleDataRow\",\"study_time\":\"14:50-16:10\",\"study_time_begin\":\"14:50\",\"study_time_end\":\"16:10\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Physics\",\"study_type\":\"Lecture\",\"cabinet\":\"005\",\"employee\":\"Great Greatness Greatier\",\"study_subgroup\":null},{\"__type\":\"VnzWeb.WidgetSchedule+ScheduleDataRow\",\"study_time\":\"16:25-17:45\",\"study_time_begin\":\"16:25\",\"study_time_end\":\"17:45\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Math\",\"study_type\":\"Practice\",\"cabinet\":\"010\",\"employee\":\"Empty Emptiness Emptier\",\"study_subgroup\":null}]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("01.11.2011"), mockResponse);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var result = await requests.GetStudentSchedule(1.November(2011), "NNN", default);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        
        var expectedLessons = new List<string> { "Physics", "Math" };
        result.Should().AllSatisfy(x => expectedLessons.Contains(x.Discipline));
    }
    
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetStudentSchedule_ReturnEmptyList()
    {
        // Arrange
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("02.12.2012"), "{\"d\":[]}");
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var result = await requests.GetStudentSchedule(2.December(2012), "NNN", default);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetStudentSchedule_ThrowsException()
    {
        // Arrange
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("10.10.2010"), "");
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetStudentSchedule(10.October(2010), "NNN", default);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }
    
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetStudentSchedule_ThrowsAnotherException()
    {
        // Arrange
        var mockHttpFactory = new MockHttpFactory().SetupException<HttpRequestException>();
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetStudentSchedule(10.October(2010), "NNN", default);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }
    #endregion

    #region GetTeacherSchedule

    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetTeacherSchedule_ReturnTeacherSchedule()
    {
        // Arrange
        var mockResponse = "{\"d\":[{\"__type\":\"VnzWeb.WidgetSchedule+ScheduleDataRow\",\"study_time\":\"14:50-16:10\",\"study_time_begin\":\"14:50\",\"study_time_end\":\"16:10\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Physics\",\"study_type\":\"Lecture\",\"cabinet\":\"005\",\"study_group\":\"NNN\",\"study_subgroup\":null},{\"__type\":\"VnzWeb.WidgetSchedule+ScheduleDataRow\",\"study_time\":\"16:25-17:45\",\"study_time_begin\":\"16:25\",\"study_time_end\":\"17:45\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Math\",\"study_type\":\"Practice\",\"cabinet\":\"010\",\"study_group\":\"NNN\",\"study_subgroup\":null}]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("01.11.2011"), mockResponse);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var result = await requests.GetTeacherSchedule(1.November(2011), "NNN", default);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        
        var expectedLessons = new List<string> { "Physics", "Math" };
        result.Should().AllSatisfy(x => expectedLessons.Contains(x.Discipline));
    }
    
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetTeacherSchedule_ReturnEmptyList()
    {
        // Arrange
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("02.12.2012"), "{\"d\":[]}");
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var result = await requests.GetTeacherSchedule(2.December(2012), "NNN", default);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetTeacherSchedule_ThrowsException()
    {
        // Arrange
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("10.10.2010"), "");
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetTeacherSchedule(10.October(2010), "NNN", default);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }
    
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetTeacherSchedule_ThrowsAnotherException()
    {
        // Arrange
        var mockHttpFactory = new MockHttpFactory().SetupException<HttpRequestException>();
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetTeacherSchedule(10.October(2010), "NNN", default);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }

    #endregion

    #region GetStudentFilters

    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetStudentFilters_ReturnDictionary()
    {
        // Arrange
        const string mockFiltersData = "{\"d\":{\"__type\":\"VnzWeb.WidgetSchedule+StudentScheduleFiltersData\",\"faculties\":[{\"Key\":\"467UNWISGQ6Z\",\"Value\":\"Aluminium\"},{\"Key\":\"7F0UOMJ9ATTN\",\"Value\":\"Oxygen\"},{\"Key\":\"IWTS0T0RHVHV\",\"Value\":\"Helium\"}],\"educForms\":[{\"Key\":\"1\",\"Value\":\"Light\"},{\"Key\":\"7\",\"Value\":\"Medium\"},{\"Key\":\"3\",\"Value\":\"Hard\"}],\"courses\":[{\"Key\":\"1\",\"Value\":\"1 course\"},{\"Key\":\"2\",\"Value\":\"2 course\"},{\"Key\":\"3\",\"Value\":\"3 course\"},{\"Key\":\"4\",\"Value\":\"4 course\"}]}}";
        
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudentScheduleFiltersData"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var filters = await requests.GetStudentFilters(CancellationToken.None);
        
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
    public async Task VnzOsvitaRequestsHandler_GetStudentFilters_ThrowsException()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":[]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudentScheduleFiltersData"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetStudentFilters(CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }
    
    #endregion

    #region GetStudyGroups

    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetStudentStudyGroups_ReturnDictionary()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":{\"__type\":\"VnzWeb.WidgetSchedule+StudyGroupsData\",\"studyGroups\":[{\"Key\":\"89YFKCWMK592\",\"Value\":\"WTF-21\"},{\"Key\":\"6U0M6S8RJNGK\",\"Value\":\"LMAO-21\"},{\"Key\":\"3STTJCMVPQZF\",\"Value\":\"IDGAF-21\"},{\"Key\":\"6O5NH5SPYHBT\",\"Value\":\"LEET-21\"}],\"studyTypes\":null}}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudyGroups"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);

        // Act
        var studyGroups = await requests.GetStudentStudyGroups("", 0, 0, CancellationToken.None);
        
        // Assert
        studyGroups.Should().NotBeNull();
        studyGroups.Should().HaveCount(4);
        studyGroups.Should().AllSatisfy(x => x.Value.Should().EndWith("-21"));
    }
    
    [Fact]
    public async Task VnzOsvitaRequestsHandler_GetStudentStudyGroups_ThrowsException()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":[]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetStudyGroups"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetStudentStudyGroups("", 0, 0, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }

    #endregion

    #region GetTeacherFaculties

    [Fact]
    public async Task VnzOsvitaRequestHandler_GetTeacherFaculties_ReturnFilters()
    {
        // Arrange
        const string mockFiltersData = "{\"d\":[{\"Key\":\"467UNWISGQ6Z\",\"Value\":\"Aluminium\"},{\"Key\":\"7F0UOMJ9ATTN\",\"Value\":\"Oxygen\"},{\"Key\":\"IWTS0T0RHVHV\",\"Value\":\"Helium\"}]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetEmployeeFaculties"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var faculties = await requests.GetTeacherFaculties(CancellationToken.None);
        
        // Assert
        faculties.Should().NotBeNull();
        faculties.Should().HaveCount(3);
        faculties.Should().ContainValues("Aluminium", "Oxygen", "Helium");
    }

    [Fact]
    public async Task VnzOsvitaRequestHandler_GetTeacherFaculties_ThrowsException()
    {
        // Arrange
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetEmployeeFaculties"), "");
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetTeacherFaculties(CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }

    #endregion

    #region GetTeacherChairs

    [Fact]
    public async Task VnzOsvitaRequestHandler_GetTeacherChairs_ReturnFilters()
    {
        // Arrange
        const string mockFiltersData = "{\"d\":{\"__type\":\"VnzWeb.WidgetSchedule+EmployeeWithUnchairedData\",\"chairs\":[{\"Key\":\"467UNWISGQ6Z\",\"Value\":\"Aluminium\"},{\"Key\":\"7F0UOMJ9ATTN\",\"Value\":\"Oxygen\"},{\"Key\":\"IWTS0T0RHVHV\",\"Value\":\"Helium\"}]}}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetEmployeeChairs"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var faculties = await requests.GetTeacherChairs("NNN", CancellationToken.None);
        
        // Assert
        faculties.Should().NotBeNull();
        faculties.Should().HaveCount(3);
        faculties.Should().ContainValues("Aluminium", "Oxygen", "Helium");
    }

    [Fact]
    public async Task VnzOsvitaRequestHandler_GetTeacherChairs_ThrowsException()
    {
        // Arrange 
        const string mockFiltersData = "{\"d\":[]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetEmployeeChairs"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetTeacherChairs("NNN", CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }

    #endregion

    #region GetTeacherEmployees
    
    [Fact]
    public async Task VnzOsvitaRequestHandler_GetTeacherEmployees_ReturnFilters()
    {
        // Arrange
        const string mockFiltersData = "{\"d\":[{\"Key\":\"467UNWISGQ6Z\",\"Value\":\"Aluminium\"},{\"Key\":\"7F0UOMJ9ATTN\",\"Value\":\"Oxygen\"},{\"Key\":\"IWTS0T0RHVHV\",\"Value\":\"Helium\"}]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetEmployees"), mockFiltersData);
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var faculties = await requests.GetTeacherEmployees("NNN", "FYW", CancellationToken.None);
        
        // Assert
        faculties.Should().NotBeNull();
        faculties.Should().HaveCount(3);
        faculties.Should().ContainValues("Aluminium", "Oxygen", "Helium");
    }

    [Fact]
    public async Task VnzOsvitaRequestHandler_GetTeacherEmployees_ThrowsException()
    {
        // Arrange 
        var mockHttpFactory = new MockHttpFactory()
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("GetEmployees"), "");
        var requests = new VnzOsvitaRequestsHandler(mockHttpFactory);
        
        // Act
        var act = async () => await requests.GetTeacherEmployees("NNN", "FYW", CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }

    #endregion
}