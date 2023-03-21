using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Presistence;

namespace Notes.Tests.Common;

/// <summary>
/// статический класс содержащий методы для создания и удаления контекста для тестирования
/// используется InMemoryDataBase, для этого устанавливается соответствующий Nuget пакет
/// </summary>
public static class NotesContextFactory
{
    public static Guid UserAId = Guid.NewGuid();
    public static Guid UserBId = Guid.NewGuid();

    public static Guid NoteIdForDelete = Guid.NewGuid();
    public static Guid NoteIdForUpdate = Guid.NewGuid();

    public static NotesDbContext Create()
    {
        //создается контекст options и с его помощью создается NotesDbContext
        var options = new DbContextOptionsBuilder<NotesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new NotesDbContext(options);
        context.Database.EnsureCreated();
        context.Notes.AddRange(
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Detail1",
                EditDate = null,
                //для создания Guid можно воспользоваться встроенной в VS утилитой, которая его генерирует (Меню Средства - Создать Guid)
                //Метод Parse класса Guid используется, чтобы Guid не менялся и мы моглы бы сравнивать его
                Id = Guid.Parse("DA626D80-38E6-4152-B47C-ECC8B5AD75D8"),
                Title = "Title1",
                UserId = UserAId
            },
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Detail2",
                EditDate = null,
                Id = Guid.Parse("CBD94ED1-EA28-40DF-8ED3-7482A9B5998F"),
                Title = "Title2",
                UserId = UserBId
            },
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Detail3",
                EditDate = null,
                Id = NoteIdForDelete,
                Title = "Title3",
                UserId = UserAId
            },
            new Note
            {
                CreationDate = DateTime.Today,
                Details = "Detail4",
                EditDate = null,
                Id = NoteIdForUpdate,
                Title = "Title4",
                UserId = UserBId
            });
        context.SaveChanges();
        return context;
    }

    public static void Destroy(NotesDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
