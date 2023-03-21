using FluentValidation;

namespace Notes.Application.Notes.Commands.UpdateNote;

/// <summary>
/// класс валидации это просто класс унаследованный от AbstractValidator<Т> с набором правил этого класса
/// внутри конструктора
/// </summary>
public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(updateNoteCommand => updateNoteCommand.UserId).NotEqual(Guid.Empty);
        RuleFor(updateNoteCommand => updateNoteCommand.Id).NotEqual(Guid.Empty);
        RuleFor(updateNoteCommand => updateNoteCommand.Title)
            .NotEmpty().MaximumLength(250);
    }
}
