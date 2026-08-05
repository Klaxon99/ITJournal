using FluentValidation;
using ITJournal.DTO;

namespace ITJournal.Services.Validators
{
    public class ArticleValidators
    {
        private Dictionary<Type, Type> _validators;
        private IServiceScopeFactory _scopeFactory;

        public ArticleValidators(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
            _validators = new Dictionary<Type, Type>();

            var assembly = typeof(ArticleValidators).Assembly;

            IEnumerable<Type> validatorsTypes = assembly.GetTypes()
                .Where(type =>
                type.BaseType != null
                && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>));

            foreach (Type validatorType in validatorsTypes)
            {
                Type dtoType = validatorType.BaseType.GetGenericArguments().First();
                
                _validators.Add(dtoType, validatorType);
            }
        }

        public async Task<bool> Validate<T>(T dto) where T : IArticleData
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (_validators.TryGetValue(typeof(T), out var validatorType) == false)
            {
                throw new KeyNotFoundException(nameof(dto));
            }

            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IValidator validator = (IValidator)scope.ServiceProvider.GetRequiredService(validatorType);
                var validatorContext = new ValidationContext<object>(dto);
                return (await validator.ValidateAsync(validatorContext)).IsValid;
            }
                
        }
    }
}
