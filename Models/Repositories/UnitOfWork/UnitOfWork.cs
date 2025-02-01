
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Repositories.Book;

namespace OnlineLibrary.Models.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _libraryDbContext;
        private IBookRepository _bookRepository;
        
        private bool _disposed;

        public UnitOfWork(LibraryDbContext libraryDbContext, IBookRepository bookRepository)
        {
            _libraryDbContext = libraryDbContext;
            _bookRepository = bookRepository;
        }


        // Create a new repository instance for type T
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_libraryDbContext);
        }

        // Specific repository (lazy-loaded)
        public IBookRepository BookRepository =>
            _bookRepository ??= new BookRepository(_libraryDbContext);


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
