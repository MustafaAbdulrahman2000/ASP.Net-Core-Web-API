using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Module_05.Models;

namespace Module_05.Data;

public class AppDbContext: DbContext, IDataProtectionKeyContext
{
    public DbSet<Bid>? Bids { get; set; } = null!;
    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
