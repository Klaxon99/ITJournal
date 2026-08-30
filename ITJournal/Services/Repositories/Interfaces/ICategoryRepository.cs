using ITJournal.Models;

namespace ITJournal.Services.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetCategoriesAsync(int? id = null, string? name = null);

        public Task<Category> CreateCategory(string name);

        public Task<Category?> UpdateCategory(int id, string name);

        public Task<bool> DeleteCategory(int id);
    }
}
