using OnlineLibrary.Models.Repositories.Book;
using OnlineLibrary.Models.Repositories.ReadBooks;
using OnlineLibrary.Models.Repositories.ReadingHistory;
using OnlineLibrary.Models.Repositories.Wishlist;

namespace OnlineLibrary.Models.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Generic repository access
        IRepository<T> GetRepository<T>() where T : class;

        // Specific repository access
        IBookRepository BookRepository { get; }

        IReadingHistoryRepository ReadingHistoryRepository { get; }

        IWishlistRepository WishlistRepository { get; }

        IReadBooksRepository ReadBooksRepository { get; }

        Task CommitAsync();

        Task RollbackAsync();
    }
}
