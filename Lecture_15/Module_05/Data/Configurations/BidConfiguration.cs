using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module_05.Models;

namespace Module_05.Data.Configurations;

public class BidConfiguration: IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.ToTable(name: "Bids");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Amount)
        .IsRequired()
        .HasColumnType("decimal(18,2)");

        builder
        .Property(b => b.BidDate)
        .IsRequired();

        builder.Property(b => b.FirstName)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(b => b.LastName)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(b => b.Email)
        .IsRequired()
        .HasMaxLength(254);

        builder.Property(b => b.Telephone)
        .IsRequired()
        .HasMaxLength(15);

        builder
        .Property(b => b.Address)
        .IsRequired()
        .HasMaxLength(250);

        // Indexes
        builder.HasIndex(b => b.Email);
    }
}
