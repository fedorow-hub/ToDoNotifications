using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNoteDetails;

/// <summary>
/// класс валидации это просто класс унаследованный от AbstractValidator<Т> с набором правил этого класса
/// внутри конструктора
/// </summary>
public class GetNoteDetailsQueryValidator : AbstractValidator<GetNoteDetailsQuery>
{
    public GetNoteDetailsQueryValidator()
    {
        RuleFor(note => note.Id).NotEqual(Guid.Empty);
        RuleFor(note => note.UserId).NotEqual(Guid.Empty);
    }
}
