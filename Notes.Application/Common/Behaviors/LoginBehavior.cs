using MediatR;
using Notes.Application.Interfaces;
using Serilog;

namespace Notes.Application.Common.Behaviors;

public class LoginBehavior<Trequest, TResponse>
    : IPipelineBehavior<Trequest, TResponse> where Trequest
    : IRequest<TResponse>
{
    ICurrentUserService _currentUserService;
    public LoginBehavior(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    public async Task<TResponse> Handle(Trequest request, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(Trequest).Name;
        var userId = _currentUserService.UserId;
        //оператор @ перед названием параметра говорит Serilogу сериализовать передаваемый объект, вместо того,
        //чтобы конвертировать объект в строчку, вызывая метод ToString
        //Имя запроса мы можем получить основываясь на информации о типе, но нам также нужен ID пользователя
        //для этого создадим отдельный маленький сервис (папка Interfaces, интерфейс ICurrentUserService, реализация
        //которого будет в проекте Notes.WebAPI в папке Services)
        Log.Information("Notes Request: {Name} {@UserId} {@Request}",
            requestName, userId, request);

        var response = await next();

        return response;
    }
}
