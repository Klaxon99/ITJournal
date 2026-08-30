using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services
{
    public class UsersQueryBuilder
    {
        public IQueryable<User> Build(IQueryable<User> query, UsersFilter usersFilter)
        {
            query = query
                .WhereIf(usersFilter.Id != null, user => user.Id == usersFilter.Id)
                .WhereIf(usersFilter.Username != null, user => user.Username == usersFilter.Username)
                .WhereIf(usersFilter.Email != null, user => user.Email == usersFilter.Email)
                .Paginate(skip: usersFilter.skip, take: usersFilter.limit);

            return query;
        }
    }

    public class ArticleQueryBuilder
    {
        public IQueryable<Article> Build(IQueryable<Article> query, ArticlesFilterRequest filter)
        {
            query = query
                .WhereIf(filter.Id != null, article => article.Id == filter.Id)
                .WhereIf(string.IsNullOrEmpty(filter.Title) == false, article => article.Title == filter.Title)
                .WhereIf(filter.AuthorId != null, article => article.AuthorId == filter.AuthorId)
                .WhereIf(filter.CategoriesIds.Count > 0, article => article.Categories
                    .Where(category => filter.CategoriesIds.Contains(category.Id))
                    .Count() == filter.CategoriesIds.Count)
                .Include(article => article.Author);

            return query;
        }
    }

    public class CommentQueryBuilder
    {
        public IQueryable<Comment> Build(IQueryable<Comment> query, CommentsFilterRequest filter)
        {
            return query
                .WhereIf(filter.Id != null, comment => comment.Id == filter.Id)
                .WhereIf(filter.AticleId != null, comment => comment.ArticleId == filter.AticleId)
                .WhereIf(filter.ParentId != null, comment => comment.ParentId == filter.ParentId)
                .WhereIf(filter.AuthorId != null, comment => comment.AuthorId == filter.AuthorId);
        }
    }
}
