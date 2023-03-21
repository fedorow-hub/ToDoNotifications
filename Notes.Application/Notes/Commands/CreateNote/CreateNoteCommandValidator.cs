using FluentValidation;

namespace Notes.Application.Notes.Commands.CreateNote;

/// <summary>
/// класс валидации это просто класс унаследованный от AbstractValidator<Т> с набором правил этого класса
/// внутри конструктора
/// </summary>
public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(createNoteCommand =>
            createNoteCommand.Title).NotEmpty().MaximumLength(250);
        RuleFor(createNoteCommand =>
            createNoteCommand.UserId).NotEqual(Guid.Empty);
    }
}
