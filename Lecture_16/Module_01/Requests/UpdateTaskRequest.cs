using Module_01.Enums;

namespace Module_01.Requests
{
	public class UpdateTaskRequest
	{
		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public ProjectTaskStatus Status { get; set; }
	}
}
