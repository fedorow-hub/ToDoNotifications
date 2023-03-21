using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;

namespace Notes.Presistence;

public static class DependencyInjection
{
    /// <summary>
    /// метод расширения для добавления контекста БД в WEB приложение. Мы будем использовать этот метод при
    /// настройке проекта WEB API.
    /// Метод добавляет использование контекста БД и регистрирует его.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        services.AddDbContext<NotesDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        services.AddScoped<INotesDbContext>(provider =>
            provider.GetService<NotesDbContext>());
        return services;
    }
}

//регистрация групп сервисов с помощью методов расширения. У нас есть отдельные проекты и мы хотим чтобы все,
//что в них есть, мы в последствии могли зарегистрировать в главном WEB API проекте.
//Для регистрации группы связанных сервисов на платформе ASP.Net Core используется соглашение, которое заключается
//в использовании одного метода расширения ADD{название группы} в нашем случае - AddPersistence для регистрации
//всех сервисов, необходимых компоненту платформы, например метод расширения AddControllers регистрирует сервисы,
//необходимые контроллерам MVC 
