using FluentResults;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using ITJournalDTOs.DTO;

namespace ITJournal.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly DTOValidators _validator;

        public CategoryService(ICategoryRepository repository, DTOValidators validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<T>> GetMappingItems<T>(CategoryFilter filter)
        {
            return await _repository.GetMappingCategory<T>(filter);
        }

        public async Task<Result<Category>> CreateCategory(CategoryServiceCreateData data)
        {
            Result<bool> validationResult = (await _validator.ValidateAsync(data)).ToResult();

            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            Category category = await _repository.CreateCategoryAsync(data.Name);

            return category;
        }

        public async Task<Result<bool>> DeleteCategory(CategoryServiceDeleteData data)
        {
            Result<bool> validationResult = (await _validator.ValidateAsync(data)).ToResult();

            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            return await _repository.DeleteCategoryAsync(data.CategoryId);
        }

        public async Task<Result<Category>> UpdateCategory(CategoryServiceUpdateData data)
        {
            Result<bool> validationResult = (await _validator.ValidateAsync(data)).ToResult();

            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            return (await _repository.UpdateCategoryAsync(data.CategoryId, data.Name))!;
        }
    }
}
