using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonalLifeAssistant.Application.Features.Tasks.Commands.CreateTask;
using PersonalLifeAssistant.Application.Features.Tasks.Commands.DeleteTask;
using PersonalLifeAssistant.Application.Features.Tasks.Commands.UpdateTask;
using PersonalLifeAssistant.Application.Features.Tasks.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Queries.GetTasks;

namespace PersonalLifeAssistant.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ISender _sender;

    public TasksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(CancellationToken cancellationToken)
        => Ok(await _sender.Send(new GetTasksQuery(), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new CreateTaskCommand(request), cancellationToken));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new UpdateTaskCommand(id, request), cancellationToken));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new DeleteTaskCommand(id), cancellationToken));
}
