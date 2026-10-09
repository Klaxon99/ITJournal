using ITJournal.Common;
using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ITJournal.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> expression)
        {
            return condition ? query.Where(expression) : query;
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, RequestFilterData filter)
        {
            return query.Skip(filter.Skip).Take(filter.Limit);
        }

        public static IQueryable<User> ApplyFilter(this IQueryable<User> query, UsersFilter usersFilter)
        {
            return query
                .WhereIf(usersFilter.Id != null, user => user.Id == usersFilter.Id)
                .WhereIf(usersFilter.Username != null, user => user.Username == usersFilter.Username)
                .WhereIf(usersFilter.Email != null, user => user.Email == usersFilter.Email)
                .Paginate(usersFilter);
        }

        public static IQueryable<Article> ApplyFilter(this IQueryable<Article> query, ArticlesFilterRequest filter)
        {
            return query
                .WhereIf(filter.Id != null, article => article.Id == filter.Id)
                .WhereIf(string.IsNullOrEmpty(filter.Title) == false, article => article.Title == filter.Title)
                .WhereIf(filter.AuthorId != null, article => article.AuthorId == filter.AuthorId)
                .WhereIf(filter.CategoriesIds.Count > 0, article => article.Categories
                    .Where(category => filter.CategoriesIds.Contains(category.Id))
                    .Count() == filter.CategoriesIds.Count)
                .Include(article => article.Author);
        }

        public static IQueryable<Comment> ApplyFilter(this IQueryable<Comment> query, CommentsFilterRequest filter)
        {
            return query
                .WhereIf(filter.Id != null, comment => comment.Id == filter.Id)
                .WhereIf(filter.AticleId != null, comment => comment.ArticleId == filter.AticleId)
                .WhereIf(filter.ParentId != null, comment => comment.ParentId == filter.ParentId)
                .WhereIf(filter.AuthorId != null, comment => comment.AuthorId == filter.AuthorId);
        }

        public static IQueryable<Category> ApplyFilter(this IQueryable<Category> query, CategoryFilter filter)
        {
            return query
                .WhereIf(filter.Id != null, category => category.Id == filter.Id)
                .WhereIf(filter.Name != null, category => category.Name == filter.Name)
                .Paginate(filter);
        }
    }
}
