namespace Module_01.Requests
{
	public class UpdateProjectRequest
	{
		public string Name { get; set; } = null!;
		public DateTime ExpectedStartDate { get; set; }
		public string? Description { get; set; }
	}
}
