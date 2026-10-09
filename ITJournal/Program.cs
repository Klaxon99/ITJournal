using FluentValidation;
using ITJournal.Models;
using ITJournal.Services;
using ITJournal.Services.ArticleSevice;
using ITJournal.Services.Extensions;
using ITJournal.Services.UserService;
using ITJournal.Services.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var JwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(JwtSettings["Key"]);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddOpenApi(options =>
{
    // 1. Регистрируем саму кнопку в Swagger UI (без глобальных требований)
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            Description = "Введите ваш JWT токен"
        });
        return Task.CompletedTask;
    });

    // 2. Подключаем наш выборочный трансформер для эндпоинтов
    options.AddOperationTransformer<BearerSecurityOperationTransformer>();
});
builder.Services.AddValidatorsFromAssembly(typeof(DTOValidators).Assembly);

builder.Services.AddScoped<IJwtGenerator, JwtSevice>();
builder.Services.AddScoped(typeof(AuthService));
builder.Services.AddScoped(typeof(CommentService));
builder.Services.AddScoped(typeof(UserService));
builder.Services.AddScoped(typeof(ArticleService));
builder.Services.AddScoped(typeof(CategoryService));

builder.Services.AddSingleton<DTOValidators>();
builder.Services.AddDbContext<ITJournalDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRepositories();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
