namespace CSharpApp.Application.Categories
{
    public class CategoriesService : ICategoriesService
    {
        public Task<Category> AddCategory(CreateCategory category)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Category>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
