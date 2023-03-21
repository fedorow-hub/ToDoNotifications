using MediatR;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.CreateNote;

/// <summary>
/// CreateNoteCommand - тип запроса
/// Guid - тип ответа
/// данный класс на основе информации, необходимой для создания заметки (из класса CreateNoteCommand)
/// содержит в себе логику создания
/// </summary>
public class CreateNoteCommandHandler
    : IRequestHandler<CreateNoteCommand, Guid>
{
    private readonly INotesDbContext _dbContext;

    public CreateNoteCommandHandler(INotesDbContext dbContext)
    {
        _dbContext = dbContext; //внедрение зависимостей на контекст БД ч/з конструктор
    }

    /// <summary>
    /// в данном методе интерфейса IRequestHandler содержится логика обработки команды
    /// здесь мы будем формировать заметку из нашего запроса и возвращать ID заметки
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Guid> Handle(CreateNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = new Note
        {
            UserId = request.UserId,
            Title = request.Title,
            Details = request.Details,
            Id = Guid.NewGuid(),
            CreationDate = DateTime.Now,
            EditDate = null
        };

        //добавление новой заметки в контекст
        await _dbContext.Notes.AddAsync(note, cancellationToken);
        //внесение изменений в БД
        await _dbContext.SaveChangesAsync(cancellationToken);
        return note.Id;
    }

}
