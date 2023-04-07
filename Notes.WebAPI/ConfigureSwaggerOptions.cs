using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Notes.WebAPI;

//также потребовалось установить Nuget пакет Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
//в частности в нем содержится интерфейс IApiVersionDescriptionProvider, который используется, чтобы получать 
//версию Api

/// <summary>
/// конфигурация Swagger
/// 
/// </summary>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// в данном методе мы из провайдера получаем описание версий и в цикле проходимся по каждой из них, заполняя
    /// данные для документа Swagger, который им будет сгенерирован
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var apiVersion = description.ApiVersion.ToString();

            //тут мы можем указать версию API, добавить заголовок, описание и др. информацию, как например
            //условия использования, лицензию и контакты
            options.SwaggerDoc(description.GroupName,
                new OpenApiInfo
                {
                    Version = apiVersion,
                    Title = $"Notes API {apiVersion}",
                    Description =
                    "A simple example ASP NET Core Web API. Professional way",
                    Contact = new OpenApiContact
                    {
                        Name = "Platinum Chat",
                        Email = string.Empty
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Platinum Chat"
                    }
                });

            //здесь мы добавляем SecurityDefinition и SecurityRequirement, необходимые для того, чтобы авторизовываться 
            //прямо в Swagger
            options.AddSecurityDefinition($"AuthToken {apiVersion}",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    Description = "Authorization token"
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = $"AuthToken {apiVersion}"
                        }
                    },
                    new string[] {}
                }
            });
            options.CustomOperationIds(apiDescription =>
                apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                    ? methodInfo.Name : null);
        }
    }
}
