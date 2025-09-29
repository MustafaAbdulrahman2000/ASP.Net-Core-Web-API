namespace Module_01.Interfaces;

public interface IUnitOfWork: IDisposable
{
    public IProductRepository Products { get; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
