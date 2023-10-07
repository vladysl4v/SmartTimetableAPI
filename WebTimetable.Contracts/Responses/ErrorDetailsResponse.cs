using System.Text.Json;

namespace WebTimetable.Contracts.Responses;

public class ErrorDetailsResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public Guid TraceId { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}