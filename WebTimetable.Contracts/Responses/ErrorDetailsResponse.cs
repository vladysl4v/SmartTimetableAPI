using System.Text.Json;

namespace WebTimetable.Contracts.Responses;

public class ErrorDetailsResponse
{
    public required int StatusCode { get; init; }
    public required string Message { get; init; }
    public required Guid TraceId { get; init; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}