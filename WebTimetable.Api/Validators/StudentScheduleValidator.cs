using FluentValidation;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Api.Validators;

public class StudentScheduleValidator : AbstractValidator<StudentScheduleRequest>
{
    public StudentScheduleValidator()
    {
        RuleFor(x => x.StudyGroup)
            .NotEmpty()
            .WithMessage("Study group cannot be empty.");

        RuleFor(x => x.StudyGroup)
            .MaximumLength(100)
            .WithMessage("Study group cannot be longer than 100 characters.");

        RuleFor(x => x.OutageGroup)
            .MaximumLength(100)
            .WithMessage("Outage group cannot be longer than 100 characters.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date cannot be empty.");

        RuleFor(x => x.Date)
            .Must(x=> DateTime.TryParse(x, out _))
            .WithMessage("The given value is not valid date.");
    }
}