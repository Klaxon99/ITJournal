using ITJournal.Common;
using ITJournal.Models;

namespace ITJournal.Services.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<T>> GetMappingCategory<T>(CategoryFilter filter);

        public Task<IEnumerable<Category>> GetCategoriesAsync(int? id = null, string? name = null);

        public Task<Category> CreateCategoryAsync(string name);

        public Task<Category?> UpdateCategoryAsync(int id, string name);

        public Task<bool> DeleteCategoryAsync(int id);
    }
}
