using FluentValidation;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Api.Validators;

public class AddNoteValidator : AbstractValidator<AddNoteRequest>
{
    public AddNoteValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty()
            .WithMessage("Lesson identifier cannot be empty.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message cannot be empty.");
        
        RuleFor(x => x.Message)
            .MaximumLength(256)
            .WithMessage("Message cannot be longer than 256 characters.");
    }
}