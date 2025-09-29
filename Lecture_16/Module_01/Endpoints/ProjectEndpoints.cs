using System.Security.Claims;
using Module_01.Filters;
using Module_01.Permissions;
using Module_01.Requests;
using Module_01.Responses;
using Module_01.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Module_01.Endpoints;

public static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("projects");

        group.MapPost("", CreateProject)
            .RequireAuthorization(Permission.Project.Create)
            .AddEndpointFilter<ValidationFilter<CreateProjectRequest>>()
            .Accepts<CreateProjectRequest>("application/json")
            .Produces<ProjectResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("CreateProject")
            .WithSummary("Creates a project")
            .WithDescription("Creates a new project")
            .WithTags("Projects")
            .MapToApiVersion(1, 0);

        group.MapGet("", GetProjects)
            .RequireAuthorization(Permission.Project.Read)
            .Produces<List<ProjectResponse>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("GetProjects")
            .WithSummary("Retrieves all projects")
            .WithTags("Projects")
            .MapToApiVersion(1, 0);

        group.MapGet("", GetProjectsV2)
            .RequireAuthorization(Permission.Project.Read)
            .Produces<List<ProjectResponse>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("GetProjectsV2")
            .WithSummary("Retrieves all projects based on specific currency")
            .WithTags("Projects")
            .MapToApiVersion(2, 0);

        group.MapGet("{projectId:guid}", GetProject)
            .RequireAuthorization(Permission.Project.Read)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("GetProjectById")
            .WithSummary("Retrieves a project")
            .WithDescription("Retrieves a specific project based on its id")
            .WithTags("Projects")
            .MapToApiVersion(1, 0);

        group.MapGet("{projectId:guid}", GetProjectV2)
            .RequireAuthorization(Permission.Project.Read)
            .Produces<ProjectResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("GetProjectByIdV2")
            .WithSummary("Retrieves a project using v2")
            .WithDescription("Retrieves a specific project based on its id using v2")
            .WithTags("Projects")
            .MapToApiVersion(2, 0);

        group.MapPut("{projectId:guid}", UpdateProject)
            .RequireAuthorization(Permission.Project.Update)
            .AddEndpointFilter<ValidationFilter<UpdateProjectRequest>>()
            .Accepts<UpdateProjectRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("UpdateProject")
            .WithSummary("Updates a project")
            .WithDescription("Updates the data of an existing project")
            .WithTags("Projects")
            .MapToApiVersion(1, 0);

        group.MapDelete("{projectId:guid}", DeleteProject)
            .RequireAuthorization(Permission.Project.Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("DeleteProject")
            .WithSummary("Deletes a project")
            .WithDescription("Deletes an existing project based on its id")
            .WithTags("Projects")
            .MapToApiVersion(1, 0);

        group.MapPut("{projectId:guid}/budget", UpdateBudget)
            .RequireAuthorization(Permission.Project.ManageBudget)
            .AddEndpointFilter<ValidationFilter<UpdateBudgetRequest>>()
            .Accepts<UpdateBudgetRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("UpdateBudget")
            .WithSummary("Updates a project's budget")
            .WithDescription("Updates the data of an existing project's budget")
            .WithTags("Projects")
            .MapToApiVersion(1, 0);

        group.MapPut("{projectId:guid}/completion", EndProject)
            .RequireAuthorization(Permission.Project.Update)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("EndProject")
            .WithSummary("Ends a project")
            .WithDescription("Ends an existing project")
            .WithTags("Projects")
            .MapToApiVersion(1, 0);

        group.MapPost("{projectId:guid}/tasks", CreateTask)
            .RequireAuthorization(Permission.Task.Create)
            .AddEndpointFilter<ValidationFilter<CreateTaskRequest>>()
            .Accepts<CreateTaskRequest>("application/json")
            .Produces<ProjectResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("CreateTask")
            .WithSummary("Creates a task")
            .WithDescription("Creates a new task")
            .WithTags("Tasks")
            .MapToApiVersion(1, 0);

        group.MapGet("{projectId:guid}/tasks/{taskId:guid}", GetTask)
            .RequireAuthorization(Permission.Task.Read)
            .Produces<ProjectTaskResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("GetTaskById")
            .WithSummary("Retrieves a task")
            .WithDescription("Retrieves a specific task based on its id")
            .WithTags("Tasks")
            .MapToApiVersion(1, 0);

        group.MapPut("{projectId:guid}/tasks/{taskId:guid}/status", UpdateTaskStatus)
            .RequireAuthorization(Permission.Task.UpdateStatus)
            .AddEndpointFilter<ValidationFilter<UpdateTaskStatusRequest>>()
            .Accepts<UpdateProjectRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("UpdateTaskStatus")
            .WithSummary("Updates a task status")
            .WithDescription("Updates the status of an existing task")
            .WithTags("Tasks")
            .MapToApiVersion(1, 0);

        group.MapPut("{projectId:guid}/tasks/{taskId:guid}", UpdateTask)
            .RequireAuthorization(Permission.Task.Update)
            .AddEndpointFilter<ValidationFilter<UpdateTaskRequest>>()
            .Accepts<UpdateProjectRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("UpdateTask")
            .WithSummary("Updates a task")
            .WithDescription("Updates the data of an existing task")
            .WithTags("Tasks")
            .MapToApiVersion(1, 0);

        group.MapPut("{projectId:guid}/tasks/{taskId:guid}/assignment", AssignUser)
            .RequireAuthorization(Permission.Task.AssignUser)
            .AddEndpointFilter<ValidationFilter<AssignUserToTaskRequest>>()
            .Accepts<AssignUserToTaskRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("AssignUser")
            .WithSummary("Assigns user to project's task")
            .WithDescription("Assigns an existing user to an existing project's task based on ids")
            .WithTags("Tasks")
            .MapToApiVersion(1, 0);

        group.MapDelete("{projectId:guid}/tasks/{taskId:guid}", DeleteTask)
            .RequireAuthorization(Permission.Task.Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName("DeleteTask")
            .WithSummary("Deletes a task")
            .WithDescription("Deletes an existing task based on its id")
            .WithTags("Tasks")
            .MapToApiVersion(1, 0);

        return group;
    }

    private static Guid GetUserId(HttpContext context)
        => Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
    private static async Task<Created<ProjectResponse>> CreateProject(CreateProjectRequest req, IProjectService service, HttpContext ctx)
    {
        var id = GetUserId(ctx);

        var result = await service.CreateProjectAsync(req, id);

        return TypedResults.Created($"/api/v1/projects/{result.Id}", result);
    }
    private static async Task<Ok<List<ProjectResponse>>> GetProjects(IProjectService service)
        => TypedResults.Ok(await service.GetProjectsAsync());
    private static async Task<Ok<ProjectResponse>> GetProject(Guid projectId, IProjectService service)
        => TypedResults.Ok(await service.GetProjectAsync(projectId));
    private static async Task<Ok<List<ProjectResponse>>> GetProjectsV2(IProjectService service)
    {
        var projects = await service.GetProjectsAsync();
        foreach (var p in projects) p.Currency = "USD";
        return TypedResults.Ok(projects);
    }
    private static async Task<Results<Ok<ProjectResponse>, NotFound<string>>> GetProjectV2(
        Guid projectId,
        IProjectService service)
    {
        var response = await service.GetProjectAsync(projectId);

        if (response is null)
            return TypedResults.NotFound("Project was not found");

        response.Currency = "USD";

        return TypedResults.Ok(response);
    }
    private static async Task<NoContent> UpdateProject(Guid projectId, UpdateProjectRequest request, IProjectService service, HttpContext ctx)
    {
        await service.UpdateProjectAsync(projectId, request, GetUserId(ctx));

        return TypedResults.NoContent();
    }
    private static async Task<NoContent> DeleteProject(Guid projectId, IProjectService service, HttpContext ctx)
    {
        await service.DeleteProjectAsync(projectId, GetUserId(ctx));

        return TypedResults.NoContent();
    }
    private static async Task<NoContent> UpdateBudget(Guid projectId, UpdateBudgetRequest request, IProjectService service, HttpContext ctx)
    {
        await service.ManageBudgetAsync(projectId, request, GetUserId(ctx));

        return TypedResults.NoContent();
    }
    private static async Task<NoContent> EndProject(Guid projectId, IProjectService service, HttpContext ctx)
    {
        await service.EndProjectAsync(projectId, GetUserId(ctx));

        return TypedResults.NoContent();
    }
    private static async Task<Created<ProjectTaskResponse>> CreateTask(Guid projectId, CreateTaskRequest request, IProjectService service, HttpContext ctx)
    {
        var task = await service.CreateTaskAsync(projectId, request, GetUserId(ctx));

        return TypedResults.Created($"/api/v1/projects/{projectId}/tasks/{task.Id}", task);
    }
    private static async Task<Ok<ProjectTaskResponse>> GetTask(Guid projectId, Guid taskId, IProjectService service)
        => TypedResults.Ok(await service.GetTaskAsync(projectId, taskId));
    private static async Task<NoContent> UpdateTaskStatus(Guid projectId, Guid taskId, UpdateTaskStatusRequest request, IProjectService service, HttpContext ctx)
    {
        await service.UpdateTaskStatusAsync(projectId, taskId, request, GetUserId(ctx));
        return TypedResults.NoContent();
    }
    private static async Task<NoContent> UpdateTask(Guid projectId, Guid taskId, UpdateTaskRequest request, IProjectService service, HttpContext ctx)
    {
        await service.UpdateTaskAsync(projectId, taskId, request, GetUserId(ctx));
        return TypedResults.NoContent();
    }
    private static async Task<NoContent> AssignUser(Guid projectId, Guid taskId, AssignUserToTaskRequest request, IProjectService service, HttpContext ctx)
    {
        await service.AssignUserToTaskAsync(projectId, taskId, request, GetUserId(ctx));
        return TypedResults.NoContent();
    }
    private static async Task<NoContent> DeleteTask(Guid projectId, Guid taskId, IProjectService service, HttpContext ctx)
    {
        await service.DeleteTaskAsync(projectId, taskId, GetUserId(ctx));
        return TypedResults.NoContent();
    }
}