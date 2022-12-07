using Ex04.Data.Infrastructure.Repositories;
using Ex04.Models;
using Ex04.Models.BaseEntities;

namespace Ex04.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        NewsContext DataContext { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();

        ICoreRepository<T> CoreRepository<T>() where T : BaseEntity;

        #region Master Data

        ICoreRepository<Post> PostRepository { get; }

        ICoreRepository<Category> CategoryRepository { get; }

        ICoreRepository<Comment> CommentRepository { get; }

        ICoreRepository<Rate> RateRepository { get; }

        ICoreRepository<ImageCategory> ImageCategoryRepository { get; }

        ICoreRepository<Image> ImageRepository { get; }

        ICoreRepository<@int> ImageAndCategoryRepository { get; }

        #endregion
    }
}
