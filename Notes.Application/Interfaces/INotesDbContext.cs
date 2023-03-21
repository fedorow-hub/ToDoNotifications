//данный интерфейс реализуется в проекте Notes.Persistence
//т.е. интерфейс это часть приложения, а реализация во вне

using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Application.Interfaces;

public interface INotesDbContext
{
    DbSet<Note> Notes { get; set; }//представляет коллекцию всех сущностей в контексте
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);//мы просто дублируем
    //сигнатуру этого метода из класса DBContext EF для удобства. Все что он делает,
    //это сохраняет изменение контекста в базу данных 
}
