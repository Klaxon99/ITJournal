using Microsoft.EntityFrameworkCore;

namespace ITJournal.Models
{
    public class ITJournalDbContext : DbContext
    {
        public ITJournalDbContext(DbContextOptions<ITJournalDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(user => user.Id).IsRequired();
                entity.Property(user => user.Username).IsRequired().HasMaxLength(256);
                entity.Property(user => user.Email).IsRequired().HasMaxLength(256);
                entity.HasIndex(user => user.Email);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(article => article.Id);
                entity.Property(article => article.Title).IsRequired().HasMaxLength(256);
                entity.Property(article => article.Content).IsRequired();

                entity.HasMany(entity => entity.Categories)
                .WithMany(category => category.Articles);

                entity.HasOne(article => article.Author)
                .WithMany(author => author.Articles)
                .HasForeignKey(article => article.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Text).IsRequired();

                entity.HasOne(comment => comment.Author)
                    .WithMany(author => author.Comments)
                    .HasForeignKey(comment => comment.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(comment => comment.Article)
                    .WithMany(article => article.Comments)
                    .HasForeignKey(comment => comment.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(comment => comment.ParentComment)
                    .WithMany(comment => comment.Replies)
                    .HasForeignKey(comment => comment.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.Property(category => category.Name).IsRequired().HasMaxLength(50);
            });
        }
    }
}
