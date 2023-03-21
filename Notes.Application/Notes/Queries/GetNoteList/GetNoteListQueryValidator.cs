using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNoteList;

/// <summary>
/// класс валидации это просто класс унаследованный от AbstractValidator<Т> с набором правил этого класса
/// внутри конструктора
/// </summary>
public class GetNoteListQueryValidator : AbstractValidator<GetNoteListQuery>
{
    public GetNoteListQueryValidator()
    {
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}
