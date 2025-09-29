using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module_01.Entities;

namespace Module_01.Data.Configurations
{
	public class ProjectConfiguration : IEntityTypeConfiguration<Project>
	{
		public void Configure(EntityTypeBuilder<Project> builder)
		{
			builder.HasKey(p => p.Id);

			builder.Property(p => p.Name)
				   .IsRequired()
				   .HasMaxLength(200);

			builder.Property(p => p.Description)
				   .HasMaxLength(1000);

			builder.Property(p => p.OwnerId)
				   .IsRequired();

			builder.Property(p => p.CreatedAt)
				   .IsRequired();

			builder.Property(p => p.ExpectedStartDate)
				  .IsRequired();

			builder.Property(p => p.ActualEndDate)
				   .IsRequired(false);

			builder.Property(p => p.Budget)
				   .IsRequired();

			builder.HasMany(p => p.Tasks)
					 .WithOne(t => t.Project)
					 .HasForeignKey(t => t.ProjectId)
					 .OnDelete(DeleteBehavior.Cascade);

		}
	}
}
