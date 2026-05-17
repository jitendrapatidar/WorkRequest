using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using WorkRequestTracker.Application.Common.Exceptions;
using WorkRequestTracker.Application.Common.Interfaces;
using WorkRequestTracker.Domain.Entities;

namespace WorkRequestTracker.Application.WorkRequests.Commands.AddWorkRequestNote;

public class AddWorkRequestNoteCommand : IRequest<int>
{
    public int WorkRequestId { get; set; }
    public string NoteText { get; set; } = string.Empty;
}

public class AddWorkRequestNoteCommandValidator : AbstractValidator<AddWorkRequestNoteCommand>
{
    public AddWorkRequestNoteCommandValidator()
    {
        RuleFor(v => v.WorkRequestId).GreaterThan(0);
        RuleFor(v => v.NoteText)
            .MaximumLength(1000)
            .NotEmpty();
    }
}

public class AddWorkRequestNoteCommandHandler : IRequestHandler<AddWorkRequestNoteCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddWorkRequestNoteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddWorkRequestNoteCommand request, CancellationToken cancellationToken)
    {
        var workRequest = await _context.WorkRequests.FindAsync(new object[] { request.WorkRequestId }, cancellationToken);

        if (workRequest == null)
        {
            throw new NotFoundException(nameof(WorkRequest), request.WorkRequestId);
        }

        var note = new WorkRequestNote
        {
            NoteText = request.NoteText,
            CreatedDate = DateTime.UtcNow,
            WorkRequestId = request.WorkRequestId
        };

        _context.WorkRequestNotes.Add(note);
        await _context.SaveChangesAsync(cancellationToken);

        return note.Id;
    }
}
