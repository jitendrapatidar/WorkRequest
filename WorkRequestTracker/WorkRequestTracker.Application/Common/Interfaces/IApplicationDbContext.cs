using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WorkRequestTracker.Domain.Entities;

namespace WorkRequestTracker.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<WorkRequest> WorkRequests { get; }
    DbSet<WorkRequestNote> WorkRequestNotes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
