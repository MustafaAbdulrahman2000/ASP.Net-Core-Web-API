using PaymentServiceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentServiceApi.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
       public void Configure(EntityTypeBuilder<Payment> builder)
       {
              builder.HasKey(o => o.PaymentReference);

              builder.Property(o => o.Amount)
                     .IsRequired();

              builder.Property(o => o.ProcessedAt)
                     .IsRequired();

              builder.Property(o => o.OrderId)
              .IsRequired();
       }
}