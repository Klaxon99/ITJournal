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
            SetUser(modelBuilder);
            SetArticle(modelBuilder);
            SetComment(modelBuilder);
            SetCategory(modelBuilder);
            SetLikes(modelBuilder);
        }

        private void SetLikes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ArticleId).IsRequired();
                entity.Property(x => x.AuthorId).IsRequired();

                entity.HasOne(like => like.Article)
                .WithMany(article => article.Likes)
                .HasForeignKey(like => like.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(like => like.User)
                .WithMany()
                .HasForeignKey(like => like.AuthorId)
                .OnDelete(DeleteBehavior.ClientCascade);
            });
        }

        private void SetUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(user => user.Id).IsRequired();
                entity.Property(user => user.PasswordHash).IsRequired();
                entity.Property(user => user.Role).HasConversion<string>();
                entity.Property(user => user.Username).IsRequired().HasMaxLength(256);
                entity.Property(user => user.Email).IsRequired().HasMaxLength(256);
                entity.HasIndex(user => user.Email);
            });
        }

        private void SetArticle(ModelBuilder modelBuilder)
        {
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
                .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void SetComment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Text).IsRequired();

                entity.HasOne(comment => comment.Author)
                    .WithMany(author => author.Comments)
                    .HasForeignKey(comment => comment.AuthorId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(comment => comment.Article)
                    .WithMany(article => article.Comments)
                    .HasForeignKey(comment => comment.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(comment => comment.ParentComment)
                    .WithMany(comment => comment.Replies)
                    .HasForeignKey(comment => comment.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void SetCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.Property(category => category.Name).IsRequired().HasMaxLength(50);
            });
        }
    }
}
