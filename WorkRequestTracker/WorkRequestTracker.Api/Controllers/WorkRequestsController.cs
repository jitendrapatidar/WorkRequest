using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkRequestTracker.Application.Common.Models;
using WorkRequestTracker.Application.DTOs;
using WorkRequestTracker.Application.WorkRequests.Commands.AddWorkRequestNote;
using WorkRequestTracker.Application.WorkRequests.Commands.CreateWorkRequest;
using WorkRequestTracker.Application.WorkRequests.Commands.UpdateWorkRequestStatus;
using WorkRequestTracker.Application.WorkRequests.Queries.GetWorkRequestById;
using WorkRequestTracker.Application.WorkRequests.Queries.GetWorkRequests;

namespace WorkRequestTracker.Api.Controllers;

[ApiController]
[Route("api/work-requests")]
public class WorkRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<WorkRequestDto>>> GetWorkRequests([FromQuery] GetWorkRequestsQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkRequestDto>> GetWorkRequestById(int id)
    {
        return await _mediator.Send(new GetWorkRequestByIdQuery { Id = id });
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateWorkRequest(CreateWorkRequestCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetWorkRequestById), new { id = id }, id);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateWorkRequestStatus(int id, UpdateWorkRequestStatusCommand command)
    {
         
        if (command.Id != 0 && id != command.Id)
        {
            return BadRequest(new { error = "Bad Request", message = "Id in route must match Id in body." });
        }

        command.Id = id;

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:int}/notes")]
    public async Task<ActionResult<int>> AddWorkRequestNote(int id, AddWorkRequestNoteCommand command)
    {
         
        if (command.WorkRequestId != 0 && id != command.WorkRequestId)
        {
            return BadRequest(new { error = "Bad Request", message = "Id in route must match WorkRequestId in body." });
        }

        command.WorkRequestId = id;

        var noteId = await _mediator.Send(command);
        return Ok(new { NoteId = noteId });
    }
}
