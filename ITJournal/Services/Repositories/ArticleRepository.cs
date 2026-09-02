using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ITJournalDbContext _dbContext;
        
        public ArticleRepository(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Article>> GetArticles(ArticlesFilterRequest filter)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .ApplyFilter(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetMappingArticles<T>(ArticlesFilterRequest filter)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .ApplyFilter(filter)
                .ProjectToType<T>()
                .ToListAsync();
        }

        public async Task<Article?> CreateArticle(ArticleCreateData createData)
        {
            List<Category> categories = await _dbContext.Categories
                .Where(category => createData.CategoriesIds.Contains(category.Id)).ToListAsync();
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == createData.AuthorId);

            Article article = new Article
            {
                Title = createData.Title,
                Content = createData.Content,
                AuthorId = createData.AuthorId,
                Categories = categories,
                Author = user,
                CreatedAt = DateTime.Now
            };
             
            _dbContext.Articles.Add(article);
            
            await _dbContext.SaveChangesAsync();

            return article;
        }

        public async Task<Article?> UpdateArticle(int id, ArticleUpdateData updateData)
        {
            Article? article = await _dbContext.Articles
                .Include(art => art.Author)
                .Include(art => art.Categories)
                .FirstOrDefaultAsync(art => art.Id == id);

            if (article == null)
            {
                return null;
            }

            article.Title = updateData.Title == null ? article.Title : updateData.Title;
            article.Content = updateData.Content == null ? article.Content : updateData.Content;
            article.UpdatedAt = DateTime.Now;

            if (updateData.CategoriesIds.Count != 0)
            {
                article.Categories.Clear();

                IEnumerable<Category> categories = await _dbContext.Categories.Where(cat => updateData.CategoriesIds.Contains(cat.Id)).ToListAsync();

                article.Categories.AddRange(categories);
            }

            await _dbContext.SaveChangesAsync();

            return article;
        }

        public async Task<bool> DeleteArticle(int id)
        {
            Article? article = await _dbContext.Articles.FirstOrDefaultAsync(art => art.Id == id);

            if (article == null)
            {
                return false;
            }

            _dbContext.Articles.Remove(article);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
