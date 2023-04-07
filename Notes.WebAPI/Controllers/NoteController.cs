using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebAPI.Models;

namespace Notes.WebAPI.Controllers;

//одна из проблем, которую решает MediatR это проблема DIConstructorExplosion или взрыв конструктора DI
//в контроллерах, когда количество параметров в конструкторе контроллера становится неприлично большим

[ApiVersion("1.0")] //для указания номера версии в параметре запроса query string
[ApiVersion("2.0")] //для возможности вызова контроллера при обращении к любой версии добавляют все версии
//[ApiVersionNeutral] //второй способ для возможности обращения к любой версии контроллера 
[Produces("application/json")]//данный атрибут позволяет объявить, что действия контроллера поддерживают тип содержимого ответа application/json
[Route("api/{version:apiVersion}/[controller]")] //для указания номера версии в сегменте адреса добавляется {version:apiVersion}
public class NoteController : BaseController
{
    private readonly IMapper _mapper;//здесь нам также потребуется маппер, чтобы преобразовать входные данные в команду
    public NoteController(IMapper mapper)
    {
        _mapper = mapper; //внедряем его как зависимость через конструктор
    }
    /// <summary>
    /// Get the list of notes
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /note
    /// </remarks>
    /// <returns>Returns NoteListVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpGet]
    [Authorize]//тег авторизации из пространства имен Microsoft.AspNetCore.Authorization
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteListVm>> GetAll()
    {
        var query = new GetNoteListQuery
        {
            UserId = UserId
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    /// Get the note by id
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET /note/85318A6F-DF2E-4131-B7BB-C92A109B759D
    /// </remarks>
    /// <param name="id">Note id (guide)</param>
    /// <returns>Returns NoteDetailsVm</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpGet("{id}")]
    [Authorize]//тег авторизации из пространства имен Microsoft.AspNetCore.Authorization
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NoteDetailsVm>> Get(Guid id)
    {
        var query = new GetNoteDetailsQuery
        {
            UserId = UserId,
            Id = id
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    /// Creates the note
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// POST /note
    /// {
    ///     title: "note title",
    ///     details: "note details"
    /// }
    /// </remarks>
    /// <param name="createNoteDto">CreateNoteDto object</param>
    /// <returns>Returns id (guid)</returns>
    /// <response code="201">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpPost]
    [Authorize]//тег авторизации из пространства имен Microsoft.AspNetCore.Authorization
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)//FromBody указывает, что
        //параметр метода должен быть извлечен из тела HTTP запроса и затем десериализован с помощью форматтера
        //входных данных или input форматтера (по умолчанию имеется только форматтер Json)
    {
        //формируем команду
        var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
        //и добавляем к ней UserId
        command.UserId = UserId;
        var noteId = await Mediator.Send(command);
        return Ok(noteId);
    }

    /// <summary>
    /// Updates the note
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// PUT /note
    /// {
    ///     title: "update note title"
    /// }
    /// </remarks>
    /// <param name="updateNoteDto">UpdateNoteDto object</param>
    /// <returns>Return NoContent</returns>
    /// <response code="204">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpPut]
    [Authorize]//тег авторизации из пространства имен Microsoft.AspNetCore.Authorization
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IActionResult>> Update([FromBody] UpdateNoteDto updateNoteDto)
    {
        var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete the note by id
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// DELETE /note/27B7F2B9-B9B8-4275-865C-B03BAC817882
    /// </remarks>
    /// <param name="id">Id of the note (guid)</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    [HttpDelete("{id}")]
    [Authorize]//тег авторизации из пространства имен Microsoft.AspNetCore.Authorization
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IActionResult>> Delete(Guid id)
    {
        var command = new DeleteNoteCommand
        {
            Id = id,
            UserId = UserId
        };
        await Mediator.Send(command);
        return NoContent();
    }
}
