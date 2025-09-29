namespace Module_01.Requests
{
	public class CreateProjectRequest
	{
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public DateTime ExpectedStartDate { get; set; }
		public decimal Budget { get; set; }
	}
}
