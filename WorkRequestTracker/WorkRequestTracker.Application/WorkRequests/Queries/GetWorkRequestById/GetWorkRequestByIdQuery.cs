using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkRequestTracker.Application.Common.Exceptions;
using WorkRequestTracker.Application.Common.Interfaces;
using WorkRequestTracker.Application.DTOs;
using WorkRequestTracker.Domain.Entities;

namespace WorkRequestTracker.Application.WorkRequests.Queries.GetWorkRequestById;

public class GetWorkRequestByIdQuery : IRequest<WorkRequestDto>
{
    public int Id { get; set; }
}

public class GetWorkRequestByIdQueryHandler : IRequestHandler<GetWorkRequestByIdQuery, WorkRequestDto>
{
    private readonly IApplicationDbContext _context;

    public GetWorkRequestByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkRequestDto> Handle(GetWorkRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.WorkRequests
            .Include(wr => wr.Notes)
            .AsNoTracking()
            .FirstOrDefaultAsync(wr => wr.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(WorkRequest), request.Id);
        }

        return WorkRequestDto.FromEntity(entity);
    }
}
