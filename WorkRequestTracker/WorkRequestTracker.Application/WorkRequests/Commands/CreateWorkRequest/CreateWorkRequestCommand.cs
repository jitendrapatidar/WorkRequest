using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using WorkRequestTracker.Application.Common.Interfaces;
using WorkRequestTracker.Domain.Entities;
using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Application.WorkRequests.Commands.CreateWorkRequest;

public class CreateWorkRequestCommand : IRequest<int>
{
    public string Title { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkRequestPriority Priority { get; set; }
    public DateTime DueDate { get; set; }
}

public class CreateWorkRequestCommandValidator : AbstractValidator<CreateWorkRequestCommand>
{
    public CreateWorkRequestCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(v => v.ClientName)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(v => v.Priority)
            .IsInEnum().WithMessage("Invalid Priority.");

        RuleFor(v => v.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Due date cannot be in the past.");
    }
}

public class CreateWorkRequestCommandHandler : IRequestHandler<CreateWorkRequestCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateWorkRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = new WorkRequest
        {
            Title = request.Title,
            ClientName = request.ClientName,
            Description = request.Description,
            Priority = request.Priority,
            Status = WorkRequestStatus.New, // Default status
            DueDate = request.DueDate,
            CreatedDate = DateTime.UtcNow
        };

        _context.WorkRequests.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
