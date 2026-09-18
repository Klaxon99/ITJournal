using FluentResults;
using FluentValidation.Results;
using ITJournal.DTO;
using ITJournal.Models;
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
            ValidationResult validationResult = await _dTOValidators.ValidateAsync(articleData);

            if (validationResult.IsValid == false)
            {
                return new Result<Article>().WithErrors(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            return await _articleRepository.CreateArticle(articleData.Adapt<ArticleCreateData>());

        }

        public async Task<Result<bool>> DeleteArticle(ArticleServiceDeleteDto data)
        {
            ValidationResult validationResult = await _dTOValidators.ValidateAsync(data);

            if (validationResult.IsValid)
            {
                return new Result<bool>().WithErrors(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            return await _articleRepository.DeleteArticle(data.ArticleId);
        }

        public async Task<Result<Article>> UpdateArticle(ArticleServiceUpdateDto articleUpdateDto)
        {
            ValidationResult validationResult = await _dTOValidators.ValidateAsync(articleUpdateDto);

            if (validationResult.IsValid == false)
            {
                return new Result<Article>().WithErrors(validationResult.Errors.Select(error => error.ErrorMessage));
            }

            return await _articleRepository.UpdateArticle(articleUpdateDto.ArticleId, articleUpdateDto.Adapt<ArticleUpdateData>());
        }
    }

    public class ArticleServiceCreateData
    {

    }
}
