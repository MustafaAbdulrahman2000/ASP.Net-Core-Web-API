using Module_01.Interfaces;

namespace Module_01.Repositories;

public class UnitOfWork(AppDbContext context): IUnitOfWork, IDisposable
{
    IProductRepository? _productRepository;
    public IProductRepository Products => _productRepository ??= new EFProductRepository(context);
    public async Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        return await context.SaveChangesAsync(token);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
