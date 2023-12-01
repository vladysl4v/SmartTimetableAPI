using Newtonsoft.Json;
using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Models;

namespace WebTimetable.Tests.DeserializersTests;

public class OutageFactoryTests
{
    private readonly OutageFactory _outageFactory = new();

    [Fact]
    public void OutageFactory_CreateAndPopulate_ReturnOutages()
    {
        // Arrange
        var mockResponse = "{\"1\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}},\"2\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}}}";
        
        // Act
        var allGroups =
            JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<Outage>>>>(mockResponse,
                _outageFactory);

       var sortedGroups = allGroups.ToDictionary(group => int.Parse(group.Key),
            group => group.Value.ToDictionary(item => ConvertToDayOfWeek(item.Key),
                item => item.Value.Where(x => x.IsDefinite is not null).ToList()));
       
       // Assert
       sortedGroups.Should().NotBeNull();
       sortedGroups.Should().HaveCount(2);
       sortedGroups.Should().AllSatisfy(pair => pair.Value.Should().HaveCount(7));
       sortedGroups.Should()
           .AllSatisfy(pair => pair.Value.Should()
               .AllSatisfy(x => x.Value.Should()
                   .AllSatisfy(y => y.IsDefinite.Should()
                       .NotBeNull()))); 
    }
    
    [Fact]
    public void OutageFactory_WriteJson_ThrowsException()
    {
        // Act
        var act = () => _outageFactory.WriteJson(new JsonTextWriter(new StringWriter()), "", new JsonSerializer());
        
        // Assert
        act.Should().Throw<NotSupportedException>();
    }
    
    [Fact]
    public void OutageFactory_CanWrite_ReturnFalse()
    {
        _outageFactory.CanWrite.Should().BeFalse();
    }
    
    private DayOfWeek ConvertToDayOfWeek(string value)
    {
        var integer = int.Parse(value);
        return integer == 7 ? DayOfWeek.Sunday : (DayOfWeek)integer;
    }
}