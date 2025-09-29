using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module_02.Permissions;

namespace Module_02.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController: ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permission.Project.Read)]
    public IActionResult GetProjects()
    {
        return Ok(new 
        {
            Message = "Read list of projects.", 
            UserInfo = GetUserInfo(),
            Permission = Permission.Project.Read
        });
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permission.Project.Read)]
    public IActionResult GetProjectById(string id)
    {
        return Ok(new 
        {
            Message = $"Read a single project with Id = {id}.", 
            UserInfo = GetUserInfo(),
            Permission = Permission.Project.Read
        });
    }

    [HttpPost]
    [Authorize(Policy = Permission.Project.Create)]
    public IActionResult CreateProject()
    {
        return CreatedAtAction(
            nameof(GetProjectById),
            new { id = Guid.NewGuid() },
            new
            {
                Message = "Project created successfully",
                UserInfo = GetUserInfo(),
                Permission = Permission.Project.Create // Using constant for consistency
            });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permission.Project.Update)]
    public IActionResult UpdateProject(string id)
    {
        return Ok(new 
        {
            Message = $"Project with Id = {id} was updated successfully.", 
            UserInfo = GetUserInfo(),
            Permission = Permission.Project.Update
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permission.Project.Delete)]
    public IActionResult DeleteProject(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok(new 
        {
            Message = $"Project with Id = {id} was deleted successfully.", 
            UserInfo = GetUserInfo(),
            Permission = Permission.Project.Delete
        });
    }

    [HttpPost("{id}/members")]
    [Authorize(Policy = Permission.Project.AssignMember)]
    public IActionResult AssignMember(string id)
    {
        return CreatedAtAction(
            nameof(GetProjectById),
            new { id },
            new
            {
                Message = $"A member has been assigned successfully to project '{id}'",
                UserInfo = GetUserInfo(),
                Permission = Permission.Project.AssignMember // Using constant for consistency
            }
        );
    }

    [HttpGet("{id}/budget")]
    [Authorize(Policy = Permission.Project.ManageBudget)]
    public IActionResult GetProjectBudget(string id)
    {
        return Ok(new 
        {
            Message = $"Successfully accessed the budget for project '{id}'.", 
            UserInfo = GetUserInfo(),
            Permission = Permission.Project.ManageBudget
        });
    }

    [HttpGet("tasks/{taskId}")]
    [Authorize(Policy = Permission.Task.Read)]
    public IActionResult GetTask(string taskId)
    {
        return Ok(new
        {
            Message = $"Task '{taskId}' details retrieved",
            UserInfo = GetUserInfo(),
            Permission = Permission.Task.Read // Using constant for consistency
        });
    }

    [HttpPost("tasks")]
    [Authorize(Policy = Permission.Task.Create)]
    public IActionResult CreateTask()
    {
        var taskId = Guid.NewGuid().ToString();

        return Created($"/api/projects/tasks/{taskId}", new
        {
            Message = $"Task '{taskId}' created successfully",
            UserInfo = GetUserInfo(),
            Permission = Permission.Task.Create // Using constant for consistency
        });
    }

    [HttpPost("tasks/{taskId}/assign")]
    [Authorize(Policy = Permission.Task.AssignUser)]
    public IActionResult AssignUserToTask(string taskId)
    {
        return Ok(new
        {
            Message = $"User assigned to task '{taskId}' successfully",
            UserInfo = GetUserInfo(),
            Permission = Permission.Task.AssignUser // Using constant for consistency
        });
    }

    [HttpPut("tasks/{taskId}")]
    [Authorize(Policy = Permission.Task.Update)]
    public IActionResult UpdateTask(string taskId)
    {
        return Ok(new
        {
            Message = $"Task '{taskId}' updated successfully",
            UserInfo = GetUserInfo(),
            Permission = Permission.Task.Update // Using constant for consistency
        });
    }

    [HttpDelete("tasks/{taskId}")]
    [Authorize(Policy = Permission.Task.Delete)]
    public IActionResult DeleteTask(string taskId)
    {
        return Ok(new
        {
            Message = $"Task '{taskId}' deleted successfully",
            UserInfo = GetUserInfo(),
            Permission = Permission.Task.Delete // Using constant for consistency
        });
    }

    [HttpPut("tasks/{taskId}/status")]
    [Authorize(Policy = Permission.Task.UpdateStatus)]
    public IActionResult UpdateTaskStatus(string taskId)
    {
        return Ok(new
        {
            Message = $"Successfully updated status for task '{taskId}'",
            UserInfo = GetUserInfo(),
            Permission = Permission.Task.UpdateStatus // Using constant for consistency
        });
    }

    private string GetUserInfo()
    {
        if (User.Identity is { IsAuthenticated: false })
            return "Anonymous";
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var firstName = User.FindFirst(ClaimTypes.GivenName);
        var lastName = User.FindFirst(ClaimTypes.Surname);

        return $"[{userId}] {firstName} {lastName}";
    }
}