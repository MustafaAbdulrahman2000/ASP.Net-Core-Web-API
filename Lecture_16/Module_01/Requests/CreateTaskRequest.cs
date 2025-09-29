﻿namespace Module_01.Requests
{
	public class CreateTaskRequest
	{
		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public Guid AssignedUserId { get; set; }
	}
}
