namespace Module_01.Entities
{
	public class Project
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public Guid OwnerId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime ExpectedStartDate { get; set; }
		public DateTime? ActualEndDate { get; set; }
		public decimal Budget { get; set; }

		public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
	}
}
