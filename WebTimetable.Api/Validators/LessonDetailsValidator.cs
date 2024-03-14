using FluentValidation;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Api.Validators;

public class LessonDetailsValidator : AbstractValidator<LessonDetailsRequest>
{
    public LessonDetailsValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date cannot be empty.");

        RuleFor(x => x.Date)
            .Must(x=> DateOnly.TryParse(x, out _))
            .WithMessage("The given value is not valid date.");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time cannot be empty.");

        RuleFor(x => x.StartTime)
            .Must(x=> TimeOnly.TryParse(x, out _))
            .WithMessage("The given value is not valid time.");
        
        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("Start time cannot be empty.");

        RuleFor(x => x.EndTime)
            .Must(x=> TimeOnly.TryParse(x, out _))
            .WithMessage("The given value is not valid time.");
    }
}