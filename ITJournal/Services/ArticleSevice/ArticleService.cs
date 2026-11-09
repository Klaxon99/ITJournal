using FluentResults;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using ITJournalDTOs.DTO;
using Mapster;

namespace ITJournal.Services.ArticleSevice
{
    public class ArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly DTOValidators _dTOValidators;

        public ArticleService(IArticleRepository articleRepository, DTOValidators dTOValidators)
        {
            _articleRepository = articleRepository;
            _dTOValidators = dTOValidators;
        }

        public async Task<IEnumerable<Article>> GetArticles(ArticleServiceFilter filter)
        {
            return await _articleRepository.GetArticles(filter.Adapt<ArticlesFilterRequest>());
        }

        public async Task<IEnumerable<T>> GetMappingArticles<T>(ArticleServiceFilter filter)
        {
            return await _articleRepository.GetMappingArticles<T>(filter.Adapt<ArticlesFilterRequest>());
        }

        public async Task<Result<Article>> CreateArticle(ArticleServiceCreateData articleData)
        {
            return await CallValid(
                articleData, 
                async () => await _articleRepository.CreateArticle(articleData.Adapt<ArticleCreateData>()));
        }

        public async Task<Result<bool>> DeleteArticle(ArticleServiceDeleteDto data)
        {
            return await CallValid(data, async () => await _articleRepository.DeleteArticle(data.ArticleId));
        }

        public async Task<Result<Article>> UpdateArticle(ArticleServiceUpdateDto articleUpdateDto)
        {
            return await CallValid(
                articleUpdateDto,
                async () => await _articleRepository.UpdateArticle(
                    articleUpdateDto.ArticleId, 
                    articleUpdateDto.Adapt<ArticleUpdateData>())
                );
        }

        private async Task<Result<bool>> ValidateDto<T>(T dto)
        {
            return (await _dTOValidators.ValidateAsync(dto)).ToResult();
        }

        private async Task<Result<T>> CallValid<T, D>(D dto, Func<Task<T>> task) 
        {
            Result<bool> result = await ValidateDto(dto);

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            return await task.Invoke();
        }
    }
}
