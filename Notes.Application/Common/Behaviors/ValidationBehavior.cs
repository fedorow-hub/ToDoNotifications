using FluentValidation;
using MediatR;

namespace Notes.Application.Common.Behaviors;

/// <summary>
/// чтобы во время запросов и команд наша валидация работала нужно встроить валидацию в пайплайн медиатора
/// пайплайн Behavior это реализация интерфейса IPipelineBehavior<TRequest, TResponse>, он представляет собой
/// паттерн, как например фильтры, схожий с ASP.Net MVC, т.е., когда возникает необходимость внести в приложение
/// некоторую логику, которая должна отрабатывать до вызова действий контроллера 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// метод интерфейса IPipelineBehavior
    /// </summary>
    /// <param name="request">объект запроса, переданный ч/з метод IMediatr.Send</param>
    /// <param name="next">это ассинхронное продолжение для следующего действия в цепочке вызовов нашего Behavior</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(request => request.Errors)
            .Where(failure => failure != null)
            .ToList();
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }
        return next();
    }
}
