using WorkRequestTracker.Domain.Entities;

namespace WorkRequestTracker.Application.DTOs;

public class WorkRequestDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public List<WorkRequestNoteDto> Notes { get; set; } = new();

    public static WorkRequestDto FromEntity(WorkRequest entity)
    {
        return new WorkRequestDto
        {
            Id = entity.Id,
            Title = entity.Title,
            ClientName = entity.ClientName,
            Description = entity.Description,
            Priority = entity.Priority.ToString(),
            Status = entity.Status.ToString(),
            DueDate = entity.DueDate,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
            Notes = entity.Notes?.Select(WorkRequestNoteDto.FromEntity).ToList() ?? new List<WorkRequestNoteDto>()
        };
    }
}
