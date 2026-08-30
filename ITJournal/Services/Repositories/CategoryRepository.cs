using ITJournal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ITJournal.Services.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ITJournalDbContext _dbContext;

        public CategoryRepository(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(int? id = null, string? name = null)
        {
            IQueryable<Category> query = _dbContext.Categories;

            if (id.HasValue)
            {
                query = query.Where(cat => cat.Id == id);
            }else if(name != null)
            {
                query = query.Where(cat => cat.Name == name);
            }

            return await query.ToListAsync();
        }

        public async Task<Category> CreateCategory(string name)
        {
            Category category = new Category { Name = name };

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> UpdateCategory(int id, string name)
        {
            Category? category = (await GetCategoriesAsync(id)).FirstOrDefault();

            if (category != null)
            {
                category.Name = name;

                await _dbContext.SaveChangesAsync();
            }

            return category;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == id);

            if (category == null)
            {
                return false;
            }

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
