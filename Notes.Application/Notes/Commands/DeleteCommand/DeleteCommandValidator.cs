using FluentValidation;

namespace Notes.Application.Notes.Commands.DeleteCommand;

/// <summary>
/// класс валидации это просто класс унаследованный от AbstractValidator<Т> с набором правил этого класса
/// внутри конструктора
/// </summary>
public class DeleteCommandValidator : AbstractValidator<DeleteNoteCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(deleteNoteCommand => deleteNoteCommand.Id).NotEqual(Guid.Empty);
        RuleFor(deleteNoteCommand => deleteNoteCommand.UserId).NotEqual(Guid.Empty);
    }
}
