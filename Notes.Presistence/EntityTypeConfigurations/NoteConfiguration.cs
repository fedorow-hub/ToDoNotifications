//папка EntityTypeConfigurations будет содержать все то, что относится к конфигурации сущностей
//если появятся новые сущности, то будет очень удобно расширять конфигурации для них
//
//интерфейс IEntityTypeConfiguration<Т> позволяет разделять конфигурацию для типа сущностей
//на отдельные классы, а не в методе OnModelCreating ДБ контекста, в котором мы будем просто
//использовать готовую конфигурацию

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Domain;

namespace Notes.Presistence.EntityTypeConfigurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(x => x.Id); //говорится, что Id это наш ключ
        builder.HasIndex(x => x.Id).IsUnique();//что он уникален
        builder.Property(x => x.Title).HasMaxLength(250);//ограничение заголовка 250 символов
        //здесь можно задавать и другие правила (подробнее можно узнать в документации EF)

    }

}
