using FluentValidation;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Api.Validators;

public class TeacherScheduleValidator : AbstractValidator<TeacherScheduleRequest>
{
    public TeacherScheduleValidator()
    {
        RuleFor(x => x.TeacherId)
            .NotEmpty()
            .WithMessage("Study group cannot be empty.");

        RuleFor(x => x.TeacherId)
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