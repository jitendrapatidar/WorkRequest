namespace WorkRequestTracker.Domain.Entities;

public class WorkRequestNote
{
    public int Id { get; set; }
    public string NoteText { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    
 
    public int WorkRequestId { get; set; }
    public WorkRequest WorkRequest { get; set; } = null!;
}
