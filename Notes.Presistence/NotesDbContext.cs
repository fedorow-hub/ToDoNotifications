//реализует интерфейс из проекта Notes.Application
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Domain;
using Notes.Presistence.EntityTypeConfigurations;

namespace Notes.Presistence;

public class NotesDbContext : DbContext, INotesDbContext
{
    //т.к. в классе DbContext уже есть реализация метода SaveChangesAsync, здесь она не нужна
    public DbSet<Note> Notes { get; set; }

    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //используется созданная нами конфигурация в классе NoteConfiguration
        builder.ApplyConfiguration(new NoteConfiguration());
        base.OnModelCreating(builder);
    }
}

//в приложениях ASP.Net MVC часто используют паттерн "Репозиторий", который инкапсулирует работу
//с БД, а также паттерн "Unit of work", когда репозиториев много, чтобы упростить работу.
//мы их использовать не будем, это не всегда лучший выбор, потому что EF Core и так изолирует
//код от БД DBContext действует как Unit of work DbSet действует как репозиторий, с EF Core
//можно делать тесты без репозиториев
