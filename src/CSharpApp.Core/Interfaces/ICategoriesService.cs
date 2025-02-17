
namespace CSharpApp.Core.Interfaces
{
    public interface ICategoriesService
    {
        Task<IReadOnlyCollection<Category>> GetCategories(CancellationToken ct);
        Task<Category> GetCategoryById(int id, CancellationToken ct);
        Task<Category> AddCategory(CreateCategory category, CancellationToken ct);
    }
}
