using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using WorkRequestTracker.Application.Common.Exceptions;
using WorkRequestTracker.Application.Common.Interfaces;
using WorkRequestTracker.Domain.Entities;
using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Application.WorkRequests.Commands.UpdateWorkRequestStatus;

public class UpdateWorkRequestStatusCommand : IRequest
{
    public int Id { get; set; }
    public WorkRequestStatus Status { get; set; }
}

public class UpdateWorkRequestStatusCommandValidator : AbstractValidator<UpdateWorkRequestStatusCommand>
{
    public UpdateWorkRequestStatusCommandValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
        RuleFor(v => v.Status).IsInEnum().WithMessage("Invalid Status.");
    }
}

public class UpdateWorkRequestStatusCommandHandler : IRequestHandler<UpdateWorkRequestStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkRequestStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateWorkRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WorkRequests.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(WorkRequest), request.Id);
        }

        entity.Status = request.Status;
        entity.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
