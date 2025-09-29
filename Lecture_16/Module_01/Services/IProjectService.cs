using Module_01.Requests;
using Module_01.Responses;

namespace Module_01.Services
{
	public interface IProjectService
	{
		Task AssignUserToTaskAsync(Guid projectId, Guid taskId, AssignUserToTaskRequest request, Guid currentUserId);
		Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, Guid currentUserId);
		Task<ProjectTaskResponse> CreateTaskAsync(Guid projectId, CreateTaskRequest request, Guid currentUserId);
		Task DeleteProjectAsync(Guid projectId, Guid currentUserId);
		Task DeleteTaskAsync(Guid projectId, Guid taskId, Guid currentUserId);
		Task EndProjectAsync(Guid projectId, Guid currentUserId);
		Task<ProjectResponse?> GetProjectAsync(Guid projectId);
		Task<List<ProjectResponse>> GetProjectsAsync();
		Task<ProjectTaskResponse> GetTaskAsync(Guid projectId, Guid taskId);
		Task ManageBudgetAsync(Guid projectId, UpdateBudgetRequest request, Guid currentUserId);
		Task UpdateProjectAsync(Guid projectId, UpdateProjectRequest request, Guid currentUserId);
		Task UpdateTaskAsync(Guid projectId, Guid taskId, UpdateTaskRequest request, Guid currentUserId);
		Task UpdateTaskStatusAsync(Guid projectId, Guid taskId, UpdateTaskStatusRequest request, Guid currentUserId);
	}
}
