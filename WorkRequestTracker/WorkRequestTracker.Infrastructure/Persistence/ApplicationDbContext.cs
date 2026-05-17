using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorkRequestTracker.Application.Common.Interfaces;
using WorkRequestTracker.Domain.Entities;

namespace WorkRequestTracker.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkRequest> WorkRequests => Set<WorkRequest>();
    public DbSet<WorkRequestNote> WorkRequestNotes => Set<WorkRequestNote>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        
        builder.Entity<WorkRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(100);
            
           
            entity.HasMany(e => e.Notes)
                  .WithOne(n => n.WorkRequest)
                  .HasForeignKey(n => n.WorkRequestId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<WorkRequestNote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NoteText).IsRequired().HasMaxLength(1000);
        });

        base.OnModelCreating(builder);
    }
}
