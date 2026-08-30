using ITJournal.DTO;
using ITJournal.Models;

namespace ITJournal.Services.Repositories
{
    public interface IArticleRepository
    {
        public Task<IEnumerable<Article>> GetArticles(ArticlesFilterRequest filter);

        public Task<IEnumerable<T>> GetMappingArticles<T>(ArticlesFilterRequest filter);

        public Task<Article?> CreateArticle(ArticleCreateData createData);

        public Task<Article?> UpdateArticle(int id, ArticleUpdateData updateData);

        public Task<bool> DeleteArticle(int id);
    }
}
