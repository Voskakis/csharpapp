namespace CSharpApp.Core.Interfaces;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetProducts(CancellationToken ct);
    Task<Product> GetProductById(int id, CancellationToken ct);
    Task<Product> AddProduct(CreateProduct product, CancellationToken ct);
}