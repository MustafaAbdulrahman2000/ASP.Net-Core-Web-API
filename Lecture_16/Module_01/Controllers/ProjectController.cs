using Asp.Versioning;
using Module_01.Permissions;
using Module_01.Requests;
using Module_01.Responses;
using Module_01.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Module_01.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/projects")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Tags("projects")]
public class ProjectController(IProjectService projectService) : ControllerBase
{
    private Guid GetUserId()
        => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

    [HttpPost]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Project.Create)]
    [Consumes("application/json")]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("CreateProject")]
    [EndpointSummary("Creates a project")]
    [EndpointDescription("Creates a new project")]
    public async Task<ActionResult<ProjectResponse>> CreateProject([FromBody] CreateProjectRequest request)
    {
        var userId = GetUserId();
        var result = await projectService.CreateProjectAsync(request, userId);

        return CreatedAtAction(
            nameof(GetProject),
            new { projectId = result.Id },
            result);
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Project.Read)]
    [ProducesResponseType<List<ProjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetProjects")]
    [EndpointSummary("Retrieves all projects")]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjects()
    {
        var projects = await projectService.GetProjectsAsync();
        return Ok(projects);
    }

    [HttpGet]
    [MapToApiVersion("2.0")]
    [Authorize(Permission.Project.Read)]
    [ProducesResponseType<List<ProjectResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetProjectsV2")]
    [EndpointSummary("Retrieves all projects based on specific currency")]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjectsV2()
    {
        var projects = await projectService.GetProjectsAsync();
        foreach (var project in projects)
            project.Currency = "USD";

        return Ok(projects);
    }

    [HttpGet("{projectId:guid}")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Project.Read)]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetProjectById")]
    [EndpointSummary("Retrieves a project")]
    [EndpointDescription("Retrieves a specific project by its id")]
    public async Task<ActionResult<ProjectResponse>> GetProject([FromRoute] Guid projectId)
    {
        var project = await projectService.GetProjectAsync(projectId);
        return Ok(project);
    }

    [HttpGet("{projectId:guid}")]
    [MapToApiVersion("2.0")]
    [Authorize(Permission.Project.Read)]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetProjectByIdV2")]
    [EndpointSummary("Retrieves a project using v2")]
    [EndpointDescription("Retrieves a specific project by its id using v2")]
    public async Task<ActionResult<ProjectResponse>> GetProjectV2([FromRoute] Guid projectId)
    {
        var project = await projectService.GetProjectAsync(projectId);
        if (project is null)
            return NotFound("Project was not found");

        project.Currency = "USD";
        return Ok(project);
    }

    [HttpPut("{projectId:guid}")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Project.Update)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateProject")]
    [EndpointSummary("Updates a project")]
    [EndpointDescription("Updates the data of an existing project")]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectRequest request)
    {
        await projectService.UpdateProjectAsync(projectId, request, GetUserId());
     
        return NoContent();
    }

    [HttpDelete("{projectId:guid}")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Project.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [EndpointName("DeleteProject")]
    [EndpointSummary("Deletes a project")]
    [EndpointDescription("Deletes an existing project based its id")]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid projectId)
    {
        await projectService.DeleteProjectAsync(projectId, GetUserId());
        return NoContent();
    }

    [HttpPut("{projectId:guid}/budget")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Project.ManageBudget)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateBudget")]
    [EndpointSummary("Updates a project's budget")]
    [EndpointDescription("Updates the data of an existing project's budget")]
    public async Task<IActionResult> UpdateBudget([FromRoute] Guid projectId, [FromBody] UpdateBudgetRequest request)
    {
        await projectService.ManageBudgetAsync(projectId, request, GetUserId());
        return NoContent();
    }

    [HttpPut("{projectId:guid}/completion")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Project.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("EndProject")]
    [EndpointSummary("Ends a project")]
    [EndpointDescription("Ends an existing project")]
    public async Task<IActionResult> EndProject([FromRoute] Guid projectId)
    {
        await projectService.EndProjectAsync(projectId, GetUserId());
        return NoContent();
    }

    // === TASK ENDPOINTS ===

    [HttpPost("{projectId:guid}/tasks")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Task.Create)]
    [Consumes("application/json")]
    [ProducesResponseType<ProjectTaskResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("CreateTask")]
    [EndpointSummary("Creates a task")]
    [EndpointDescription("Creates a new task")]
    [Tags("Tasks")]
    public async Task<ActionResult<ProjectTaskResponse>> CreateTask([FromRoute] Guid projectId, [FromBody] CreateTaskRequest request)
    {
        var task = await projectService.CreateTaskAsync(projectId, request, GetUserId());
        return CreatedAtAction(nameof(GetTask), new { projectId, taskId = task.Id }, task);
    }

    [HttpGet("{projectId:guid}/tasks/{taskId:guid}")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Task.Read)]
    [ProducesResponseType<ProjectTaskResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("GetTaskById")]
    [EndpointSummary("Retrieves a task")]
    [EndpointDescription("Retrieves a specific task by its id")]
    [Tags("Tasks")]
    public async Task<ActionResult<ProjectTaskResponse>> GetTask([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        var task = await projectService.GetTaskAsync(projectId, taskId);
        return Ok(task);
    }

    [HttpPut("{projectId:guid}/tasks/{taskId:guid}/status")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Task.UpdateStatus)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateTaskStatus")]
    [EndpointSummary("Updates a task status")]
    [EndpointDescription("Updates the status of an existing task")]
    [Tags("Tasks")]
    public async Task<IActionResult> UpdateTaskStatus([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] UpdateTaskStatusRequest request)
    {
        await projectService.UpdateTaskStatusAsync(projectId, taskId, request, GetUserId());
        return NoContent();
    }

    [HttpPut("{projectId:guid}/tasks/{taskId:guid}")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Task.Update)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("UpdateTask")]
    [EndpointSummary("Updates a task")]
    [EndpointDescription("Updates the data of an existing task")]
    [Tags("Tasks")]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] UpdateTaskRequest request)
    {
        await projectService.UpdateTaskAsync(projectId, taskId, request, GetUserId());
        return NoContent();
    }

    [HttpPut("{projectId:guid}/tasks/{taskId:guid}/assignment")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Task.AssignUser)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [EndpointName("AssignUser")]
    [EndpointSummary("Assigns user to project's task")]
    [EndpointDescription("Assigns an existing user to an existing project task based on ids")]
    [Tags("Tasks")]
    public async Task<IActionResult> AssignUser([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] AssignUserToTaskRequest request)
    {
        await projectService.AssignUserToTaskAsync(projectId, taskId, request, GetUserId());
        return NoContent();
    }

    [HttpDelete("{projectId:guid}/tasks/{taskId:guid}")]
    [MapToApiVersion("1.0")]
    [Authorize(Permission.Task.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [EndpointName("DeleteTask")]
    [EndpointSummary("Deletes a task")]
    [EndpointDescription("Deletes an existing task based its id")]
    [Tags("Tasks")]
    public async Task<IActionResult> DeleteTask([FromRoute] Guid projectId, [FromRoute] Guid taskId)
    {
        await projectService.DeleteTaskAsync(projectId, taskId, GetUserId());
        return NoContent();
    }
}