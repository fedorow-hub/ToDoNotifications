using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Common.Mappings;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Notes.WebAPI.Models;

/// <summary>
/// С клиента нам будут приходить данные о создаваемой заметке, причем клиенту не нужно знать свой Id пользователя
/// соответственно нам нужно создать оттдельную модель и смаппить ее с CreateNoteCommand
/// </summary>
public class CreateNoteDto : IMapWith<CreateNoteCommand>
{
    [Required]//мы можем помечать модель аттрибутами, чтобы на пользовательском интерфейсе Swager они отображались
    public string Title { get; set; }
    public string Details { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateNoteDto, CreateNoteCommand>()
            .ForMember(noteCommand => noteCommand.Title,
                opt => opt.MapFrom(noteDto => noteDto.Title))
            .ForMember(noteCommand => noteCommand.Details,
                opt => opt.MapFrom(noteDto => noteDto.Details));
    }
}

