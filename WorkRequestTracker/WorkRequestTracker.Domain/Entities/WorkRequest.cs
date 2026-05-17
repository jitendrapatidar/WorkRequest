using WorkRequestTracker.Domain.Enums;

namespace WorkRequestTracker.Domain.Entities;

public class WorkRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public WorkRequestPriority Priority { get; set; }
    public WorkRequestStatus Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

 
    public ICollection<WorkRequestNote> Notes { get; set; } = new List<WorkRequestNote>();
}
