using Ex04.Data.Infrastructure.Repositories;
using Ex04.Models;
using Ex04.Models.BaseEntities;

namespace Ex04.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsContext _dbContext;

        public NewsContext DataContext => _dbContext;

        public UnitOfWork(NewsContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ICoreRepository<Category> _categoryRepository;

        public ICoreRepository<Category> CategoryRepository => _categoryRepository ?? new CoreRepository<Category>(_dbContext);

        private ICoreRepository<Post> _postRepository;

        public ICoreRepository<Post> PostRepository => _postRepository ?? new CoreRepository<Post>(_dbContext);

        private ICoreRepository<Comment> _commentRepository;

        public ICoreRepository<Comment> CommentRepository => _commentRepository ?? new CoreRepository<Comment>(_dbContext);

        private ICoreRepository<Rate> _rateRepository;

        public ICoreRepository<Rate> RateRepository => _rateRepository ?? new CoreRepository<Rate>(_dbContext);

        private ICoreRepository<ImageCategory> _imageCategoryRepository;

        public ICoreRepository<ImageCategory> ImageCategoryRepository => _imageCategoryRepository ?? new CoreRepository<ImageCategory>(_dbContext);

        private ICoreRepository<Image> _imageRepository;

        public ICoreRepository<Image> ImageRepository => _imageRepository ?? new CoreRepository<Image>(_dbContext);

        private ICoreRepository<@int> _imageAndCategoryRepository;

        public ICoreRepository<@int> ImageAndCategoryRepository => _imageAndCategoryRepository ?? new CoreRepository<@int>(_dbContext);


        #region Method

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            CancellationToken cancellationToken = new CancellationToken();
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public ICoreRepository<T> CoreRepository<T>() where T : BaseEntity
        {
            return new CoreRepository<T>(_dbContext);
        }

        #endregion
    }
}
