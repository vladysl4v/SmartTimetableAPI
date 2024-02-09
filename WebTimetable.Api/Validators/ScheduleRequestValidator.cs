using FluentValidation;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Api.Validators;

public class ScheduleRequestValidator : AbstractValidator<ScheduleRequest>
{
    public ScheduleRequestValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty()
            .WithMessage("Identifier cannot be empty.");

        RuleFor(x => x.Identifier)
            .MaximumLength(100)
            .WithMessage("Study group cannot be longer than 100 characters.");
        
        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date cannot be empty.");

        RuleFor(x => x.Date)
            .Must(x=> DateTime.TryParse(x, out _))
            .WithMessage("The given value is not valid date.");
    }
}