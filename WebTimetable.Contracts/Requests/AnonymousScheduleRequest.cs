using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace WebTimetable.Contracts.Requests;

public class AnonymousScheduleRequest
{
    /// <summary>
    /// Identifier of user study group.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("studyGroupID")]
    public required string StudyGroup { get; init; }

    /// <summary>
    /// Number of outage group, or leave zero to skip.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify outage group or left zero")]
    [JsonPropertyName("outageGroup")]
    public required int OutageGroup { get; init; }

    /// <summary>
    /// The date from which to start the schedule.<br/>
    /// Required date format is 'YYYY-MM-DDTHH:mm:ss' (time will be ignored)
    /// </summary>
    /// <remarks>
    /// In JavaScript use {Date}.toUTCString()
    /// </remarks>
    [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify date in 'YYYY-MM-DDTHH:mm:ss' format")]

    [JsonPropertyName("startDate")]
    public required DateTime Start { get; init; }

    /// <summary>
    /// The date until which to look for the schedule.<br/>
    /// Required date format is 'YYYY-MM-DDTHH:mm:ss' (time will be ignored)
    /// </summary>
    /// <remarks>
    /// In JavaScript use {Date}.toUTCString()
    /// </remarks>
    [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify date in 'YYYY-MM-DDTHH:mm:ss' format")]
    [JsonPropertyName("endDate")]
    public required DateTime End { get; init; }
}