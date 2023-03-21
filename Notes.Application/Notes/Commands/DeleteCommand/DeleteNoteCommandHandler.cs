using MediatR;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteCommand;

/// <summary>
/// DeleteNoteCommand - тип запроса
/// данный класс на основе информации, необходимой для изменения заметки (из класса DeleteNoteCommand)
/// содержит в себе логику изменения
/// </summary>
public class DeleteNoteCommandHandler
    : IRequestHandler<DeleteNoteCommand>
{
    private readonly INotesDbContext _dbContext;

    public DeleteNoteCommandHandler(INotesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// в данном методе интерфейса IRequestHandler содержится логика обработки команды
    /// здесь мы будем выполнять поиск сущности по Id заметки из нашего контекста БД,
    /// если заметка не найдена или Id пользователя не совпадает с Id пользователя в запросе
    /// то будет выдаваться исключение, что сущность не найдена
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Notes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Note), request.Id);
        }

        //удаление заметки из контекста
        _dbContext.Notes.Remove(entity);

        //внесение изменений в БД
        await _dbContext.SaveChangesAsync(cancellationToken);

        //Unit это тип, означающий пустой ответ
        return Unit.Value;
    }
}
