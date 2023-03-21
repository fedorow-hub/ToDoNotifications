using MediatR;

namespace Notes.Application.Notes.Queries.GetNoteDetails;

public class GetNoteDetailsQuery : IRequest<NoteDetailsVm>
{
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
}

//Здесь используется маппинг с использованием класса NoteDetailsVm, который описывает то,
//что будет возвращаться пользователю, когда он будет запрашивать детали заметки
