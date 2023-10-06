using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTimetable.Contracts.Requests;

public class PersonalScheduleRequest
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
    /// The date from which to start the schedule.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify date")]

    [JsonPropertyName("startDate")]
    public required string StartDate { get; init; }

    /// <summary>
    /// The date until which to look for the schedule.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify date")]
    [JsonPropertyName("endDate")]
    public required string EndDate { get; init; }

    public DateTime Start => DateTime.Parse(StartDate);
    public DateTime End => DateTime.Parse(EndDate);
}