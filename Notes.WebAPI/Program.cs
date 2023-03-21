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

//������������� AutoMapper �����, � �� � ������� Notes.Application, ������, ��� ��� ����� �������� ����������
//� ������� ������������� ������
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});


builder.Services.AddApplication();
builder.Services.AddPersistence(configuration);
builder.Services.AddControllers();

//��� ����� ���� ������ �� �������� �������� ���� ������, ��� ������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

//���������� �������������� � ����������
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7245/";//����� Identity server
        options.Audience = "NotesWebAPI";//������������ �� ����� Api ������� �� ������������ Identity server
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddVersionedApiExplorer(options =>
    options.GroupNameFormat = "'v'VVV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
    ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

//��������� ���������������
builder.Services.AddApiVersioning();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        //�������� �������� ��
        var context = serviceProvider.GetRequiredService<NotesDbContext>();
        //������������� �� � ������� ����� ���������� ������ DbInitializer
        DbInitializer.Initialize(context);
    }
    catch (Exception exeption)
    {
    }
}

app.UseSwagger();
//��������� Swagger � �������� �������� ����������
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

//������� �����!
app.UseAuthentication();//�������������� ��������� ����� �� ������������ � ���������������� ������� ����� � �������
app.UseAuthorization();//����������� ��� �������� �� ������� ���� ��������� �� ��� ���� ��������

app.UseApiVersioning();//��������� ���������������

//������ UseEndpoints ����� �������, ����� ������� �������� �� �������� ������������ � �� ������
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
