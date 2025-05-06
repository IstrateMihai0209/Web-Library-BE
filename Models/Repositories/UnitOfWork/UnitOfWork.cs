using OnlineLibrary.Models.Repositories.Book;
using OnlineLibrary.Models.Repositories.ReadBooks;
using OnlineLibrary.Models.Repositories.ReadingHistory;
using OnlineLibrary.Models.Repositories.Wishlist;

namespace OnlineLibrary.Models.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _libraryDbContext;
        private IBookRepository _bookRepository;
        private IReadingHistoryRepository _readingHistoryRepository;
        private IWishlistRepository _wishlistRepository;
        private IReadBooksRepository _readBooksRepository;

        private bool _disposed;

        public UnitOfWork(
            LibraryDbContext libraryDbContext,
            IBookRepository bookRepository,
            IReadingHistoryRepository readingHistoryRepository,
            IWishlistRepository wishlistRepository,
            IReadBooksRepository readBooksRepository)
        {
            _libraryDbContext = libraryDbContext;
            _bookRepository = bookRepository;
            _readingHistoryRepository = readingHistoryRepository;
            _wishlistRepository = wishlistRepository;
            _readBooksRepository = readBooksRepository;
        }


        // Create a new repository instance for type T
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_libraryDbContext);
        }

        // Specific repository (lazy-loaded)
        public IBookRepository BookRepository => _bookRepository ??= new BookRepository(_libraryDbContext);

        public IReadingHistoryRepository ReadingHistoryRepository => _readingHistoryRepository ??= new ReadingHistoryRepository(_libraryDbContext);

        public IWishlistRepository WishlistRepository => _wishlistRepository ??= new WishlistRepository(_libraryDbContext);

        public IReadBooksRepository ReadBooksRepository => _readBooksRepository ??= new ReadBooksRepository(_libraryDbContext);

        public async Task CommitAsync()
        {
            await using var transaction = await _libraryDbContext.Database.BeginTransactionAsync();
            try
            {
                await _libraryDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await _libraryDbContext.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _libraryDbContext.Dispose();
            _disposed = true;
        }
    }
}
