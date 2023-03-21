using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Presistence;
using Notes.WebAPI.Middleware;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Notes.WebAPI;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//конфигурируем AutoMapper здесь, а не в проекте Notes.Application, потому, что нам нужно получить информацию
//о текущей выполняющейся сборке
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});


builder.Services.AddApplication();
builder.Services.AddPersistence(configuration);
builder.Services.AddControllers();

//для теста пока делаем по простому разрешая кому угодно, что угодно
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

//добавление аутентификации в приложении
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7245/";//адрес Identity server
        options.Audience = "NotesWebAPI";//выставляется по имени Api ресурса из конфигурации Identity server
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddVersionedApiExplorer(options =>
    options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
    ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

//добавляет версионирование
builder.Services.AddApiVersioning();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        //получаем контекст БД
        var context = serviceProvider.GetRequiredService<NotesDbContext>();
        //инициализация БД с помощью ранее созданного класса DbInitializer
        DbInitializer.Initialize(context);
    }
    catch (Exception exeption)
    {
    }
}

app.UseSwagger();
//интерфейс Swagger в корневом каталоге приложения
app.UseSwaggerUI(config =>
{
    foreach(var description in provider.ApiVersionDescriptions)
    {
        config.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
        config.RoutePrefix = string.Empty;
    }    
});
app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

//порядок важен!
app.UseAuthentication();//аутентификация проверяет может ли пользователь с предоставленными данными войти в систему
app.UseAuthorization();//авторизация это проверка на наличие прав выполнять те или иные действия

app.UseApiVersioning();//добавляет версионирование

//меняем UseEndpoints таким образом, чтобы роутинг маппился на название контроллеров и их методы
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
