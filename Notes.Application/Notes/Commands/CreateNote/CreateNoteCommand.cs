using MediatR;

namespace Notes.Application.Notes.Commands.CreateNote;

/// <summary>
/// данный класс содержит только информацию о том, что необходимо для создания заметки
/// </summary>
public class CreateNoteCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }
}
//IRequest<T> из библиотеки MediatR, который просто помечает результат выполнения данной команды
//и вернет нам результат определенного типа 
