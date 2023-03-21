using Notes.Presistence;

namespace Notes.Tests.Common;

/// <summary>
/// базовый класс для тестирования команд
/// </summary>
public abstract class TestCommandBase : IDisposable
{
    protected readonly NotesDbContext Context;

    public TestCommandBase()
    {
        Context = NotesContextFactory.Create();
    }
    public void Dispose()
    {
        NotesContextFactory.Destroy(Context);
    }
}
