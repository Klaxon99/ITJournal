using FluentResults;
using FluentValidation.Results;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
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
