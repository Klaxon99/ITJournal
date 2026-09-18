using ITJournal.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITJournal.Services
{
    public class JwtSevice : IJwtGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtSevice(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, UserRole role)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey
            (
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSettings:Key"))
            );
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                signingCredentials: creds,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

public class BearerSecurityOperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            // 1. Проверяем, есть ли у метода или его контроллера атрибут [Authorize]
            // При этом учитываем, нет ли там переопределяющего атрибута [AllowAnonymous]
            var hasAuthorize = context.Description.ActionDescriptor.EndpointMetadata
                .Any(metadata => metadata is IAuthorizeData);

            var hasAllowAnonymous = context.Description.ActionDescriptor.EndpointMetadata
                .Any(metadata => metadata is IAllowAnonymous);

            // 2. Если эндпоинт защищен и не является публичным
            if (hasAuthorize && !hasAllowAnonymous)
            {
                // Инициализируем список требований, если он пуст
                operation.Security ??= new List<OpenApiSecurityRequirement>();

                // Добавляем требование авторизации конкретно к этой операции
                var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme // Должен совпадать с ID схемы в DocumentTransformer
                        }
                    },
                    Array.Empty<string>()
                }
            };

                operation.Security.Add(securityRequirement);
            }

            return Task.CompletedTask;
        }
    }

}
