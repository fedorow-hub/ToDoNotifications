using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails;

/// <summary>
///данный класс описывает то, что будет возвращаться пользователю, 
///когда он будет запрашивать детали заметки. Данный класс будет резализовывать
///дженерик интерфейс IMapWith<T> с реализованным по умолчанию методом Mapping
///
/// Запись данного класса и интерфейсом легко читается "вью модель заметки маппить с заметкой"
/// данный класс содержит все те же поля, что и класс заметки, кроме Id пользователя
/// </summary>
public class NoteDetailsVm : IMapWith<Note>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }
    public DateTime CreationDate { get; set; }

    public DateTime? EditDate { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Note, NoteDetailsVm>()
            .ForMember(noteVm => noteVm.Title,
                opt => opt.MapFrom(note => note.Title))
            .ForMember(noteVm => noteVm.Details,
                opt => opt.MapFrom(note => note.Details))
            .ForMember(noteVm => noteVm.Id,
                opt => opt.MapFrom(note => note.Id))
            .ForMember(noteVm => noteVm.CreationDate,
                opt => opt.MapFrom(note => note.CreationDate))
            .ForMember(noteVm => noteVm.EditDate,
                opt => opt.MapFrom(note => note.EditDate));
    }
}
