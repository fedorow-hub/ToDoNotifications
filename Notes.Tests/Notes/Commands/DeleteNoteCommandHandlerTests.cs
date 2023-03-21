using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Tests.Common;

namespace Notes.Tests.Notes.Commands;

public class DeleteNoteCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task DeleteNoteCommandHandler_Success()
    {
        //Arrange
        var handler = new DeleteNoteCommandHandler(Context);

        //Act
        await handler.Handle(new DeleteNoteCommand
        {
            Id = NotesContextFactory.NoteIdForDelete,
            UserId = NotesContextFactory.UserAId
        }, CancellationToken.None);

        //Assert
        Assert.Null(Context.Notes.SingleOrDefault(note =>
            note.Id == NotesContextFactory.NoteIdForDelete));
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new DeleteNoteCommandHandler(Context);

        //Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        await handler.Handle(
            new DeleteNoteCommand
            {
                Id = Guid.NewGuid(),
                UserId = NotesContextFactory.UserAId
            },
            CancellationToken.None));
    }

    [Fact]
    public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
    {
        //Arrange
        var deleteHandler = new DeleteNoteCommandHandler(Context);

        //Act
        //Assert      
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await deleteHandler.Handle(
                new DeleteNoteCommand
                {
                    Id = Guid.Parse("DA626D80-38E6-4152-B47C-ECC8B5AD75D8"),
                    UserId = NotesContextFactory.UserBId
                }, CancellationToken.None));
    }

}
