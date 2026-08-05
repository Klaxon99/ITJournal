using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssembly(typeof(ArticleValidators).Assembly);
builder.Services.AddSingleton<ArticleValidators>();
builder.Services.AddDbContext<ITJournalDbContext>(opt => opt.UseInMemoryDatabase("ITJournal"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
