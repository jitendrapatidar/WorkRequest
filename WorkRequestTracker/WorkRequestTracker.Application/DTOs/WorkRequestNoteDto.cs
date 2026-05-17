using System;
using WorkRequestTracker.Domain.Entities;

namespace WorkRequestTracker.Application.DTOs;

public class WorkRequestNoteDto
{
    public int Id { get; set; }
    public string NoteText { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    public static WorkRequestNoteDto FromEntity(WorkRequestNote note)
    {
        return new WorkRequestNoteDto
        {
            Id = note.Id,
            NoteText = note.NoteText,
            CreatedDate = note.CreatedDate
        };
    }
}
