using OnlineLibrary.Models.ReadBooks;

namespace OnlineLibrary.Models.Repositories.ReadBooks
{
    public interface IReadBooksRepository : IRepository<ReadBooksModel>
    {
        Task<ReadBooksModel> GetOrCreateAllReadBooksByUser(string userId);

        Task<ReadBooksModel> GetReadBooksSectionByUser(string userId);

        Task<ReadBooksModel> GetOrCreateReadBooksByUser(string userId, int pageNumber);
    }
}
