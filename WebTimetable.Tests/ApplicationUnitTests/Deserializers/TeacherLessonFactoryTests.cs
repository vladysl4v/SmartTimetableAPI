using Newtonsoft.Json;
using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Models;

namespace WebTimetable.Tests.ApplicationUnitTests.Deserializers;

public class TeacherLessonFactoryTests
{
    private readonly TeacherLessonFactory _teacherLessonFactory = new();

    [Fact]
    public void TeacherLessonFactory_CreateAndPopulate_ReturnLessons()
    {
        // Arrange
        var mockResponse = "{\"d\":[{\"__type\":\"VnzWeb.BetaSchedule+ScheduleDataRow\",\"study_time\":\"14:50-16:10\",\"study_time_begin\":\"14:50\",\"study_time_end\":\"16:10\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Physics\",\"study_type\":\"Lecture\",\"cabinet\":\"005\",\"study_group\":\"NNN\",\"study_subgroup\":null},{\"__type\":\"VnzWeb.BetaSchedule+ScheduleDataRow\",\"study_time\":\"16:25-17:45\",\"study_time_begin\":\"16:25\",\"study_time_end\":\"17:45\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Math\",\"study_type\":\"Practice\",\"cabinet\":\"010\",\"study_group\":\"NNN\",\"study_subgroup\":null}]}";
        var expected = new TeacherLesson
        {
            Start = new TimeOnly(14, 50),
            End = new TimeOnly(16, 10),
            Date = new DateOnly(2011, 11, 11),
            Discipline = "Physics",
            StudyType = "Lecture",
            Cabinet = "005",
            StudyGroup = "NNN",
        };
        
        // Act
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<TeacherLesson>>>(mockResponse, _teacherLessonFactory);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey("d");
        
        if (result != null && result.TryGetValue("d", out List<TeacherLesson> outList))
        {
            outList.Should().HaveCount(2);
            outList.Should().NotBeNull();
            outList.Should().ContainEquivalentOf(expected);
        }
    }
    
    [Fact]
    public void TeacherLessonFactory_CreateAndPopulate_ReturnEmptyList()
    {
        // Arrange
        var mockResponse = "{\"d\":[]}";
        
        // Act
        var result = JsonConvert.DeserializeObject<Dictionary<string, List<TeacherLesson>>>(mockResponse, _teacherLessonFactory);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey("d");
        
        if (result != null && result.TryGetValue("d", out List<TeacherLesson> outList))
        {
            outList.Should().NotBeNull();
            outList.Should().BeEmpty();
        }
    }

    [Fact]
    public void TeacherLessonFactory_WriteJson_ThrowsException()
    {
        // Act
        var act = () => _teacherLessonFactory.WriteJson(new JsonTextWriter(new StringWriter()), "", new JsonSerializer());
        
        // Assert
        act.Should().Throw<NotSupportedException>();
    }
    
    [Fact]
    public void TeacherLessonFactory_CanWrite_ReturnFalse()
    {
        _teacherLessonFactory.CanWrite.Should().BeFalse();
    }
}