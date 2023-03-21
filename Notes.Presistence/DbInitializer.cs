namespace Notes.Presistence;

//данный класс используется только при старте приложения и проверяет, создана ли БД и если нет,
//БД будет создана на основе нашего контекста
public class DbInitializer
{
    public static void Initialize(NotesDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
