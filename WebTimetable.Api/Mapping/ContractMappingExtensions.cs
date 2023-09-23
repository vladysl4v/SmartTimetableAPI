using WebTimetable.Contracts.Responses;

using WebTimetable.Application.Entities;


namespace WebTimetable.Api.Mapping;

public static class ContractMappingExtensions
{
    public static UserResponse MapToResponse(this UserEntity user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            OutagesGroup = user.OutagesGroup,
            StudyGroupId = user.StudyGroupId
        };
    }
}