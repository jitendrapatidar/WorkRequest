using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkRequestTracker.Application.Common.Interfaces;
using WorkRequestTracker.Application.Common.Models;
using WorkRequestTracker.Application.DTOs;
using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Application.WorkRequests.Queries.GetWorkRequests;

public class GetWorkRequestsQuery : IRequest<PaginatedList<WorkRequestDto>>
{
    public string? Status { get; set; }
    public string? Search { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetWorkRequestsQueryHandler : IRequestHandler<GetWorkRequestsQuery, PaginatedList<WorkRequestDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWorkRequestsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<WorkRequestDto>> Handle(GetWorkRequestsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.WorkRequests.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Status) && 
            System.Enum.TryParse(typeof(WorkRequestStatus), request.Status, true, out var status))
        {
            query = query.Where(wr => wr.Status == (WorkRequestStatus)status);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(wr => wr.Title.Contains(request.Search) || wr.ClientName.Contains(request.Search));
        }

        query = query.OrderByDescending(wr => wr.CreatedDate);

        var count = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(wr => new WorkRequestDto
            {
                Id = wr.Id,
                Title = wr.Title,
                ClientName = wr.ClientName,
                Description = wr.Description,
                Priority = wr.Priority.ToString(),
                Status = wr.Status.ToString(),
                DueDate = wr.DueDate,
                CreatedDate = wr.CreatedDate,
                UpdatedDate = wr.UpdatedDate
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<WorkRequestDto>(items, count, request.PageNumber, request.PageSize);
    }
}
