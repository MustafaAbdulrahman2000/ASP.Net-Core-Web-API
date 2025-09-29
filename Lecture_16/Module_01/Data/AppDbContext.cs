using Microsoft.EntityFrameworkCore;
using Module_01.Entities;

namespace Module_01.Data
{
	public class AppDbContext: DbContext
	{
		public DbSet<Project> Projects { get; set; } = null!;
		public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;

		public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
		{				
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}
	}
}
