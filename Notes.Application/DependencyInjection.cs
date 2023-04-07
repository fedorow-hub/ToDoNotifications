using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Common.Behaviors;
using System.Reflection;

namespace Notes.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //регистрация сервиса MediatR с помощью метода AddMediatR из Nuget пакета
        //MediatR.Extensions.Microsoft.DependencyInjection
        services.AddMediatR(Assembly.GetExecutingAssembly());//в метод передается выполняемая сборка 
        //добавляем валидатор из сборки
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        //а также регистрируем наш PipelineBehavior
        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));
        //добавляем логирование в Pipeline медиатора
        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(LoginBehavior<,>));
        return services;
    }
}
//регистрация групп сервисов с помощью методов расширения. У нас есть отдельные проекты и мы хотим чтобы все,
//что в них есть, мы в последствии могли зарегистрировать в главном WEB API проекте.
//Для регистрации группы связанных сервисов на платформе ASP.Net Core используется соглашение, которое заключается
//в использовании одного метода расширения ADD{название группы} в нашем случае - AddApplication для регистрации
//всех сервисов, необходимых компоненту платформы, например метод расширения AddControllers регистрирует сервисы,
//необходимые контроллерам MVC 
