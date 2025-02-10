using OnlineLibrary.Models.Repositories.Book;

namespace OnlineLibrary.Models.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Generic repository access
        IRepository<T> GetRepository<T>() where T : class;

        // Specific repository access
        IBookRepository BookRepository { get; }

        Task CommitAsync();

        Task RollbackAsync();
    }
}
