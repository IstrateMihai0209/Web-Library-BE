using OnlineLibrary.Models.ReadBooks;

namespace OnlineLibrary.Models.Repositories.ReadBooks
{
    public interface IReadBooksRepository : IRepository<ReadBooksModel>
    {
        Task<ReadBooksModel> GetAllReadBooksByUser(int userId);

        Task<ReadBooksModel> GetReadBooksSectionByUser(int userId);

        Task<ReadBooksModel> GetReadBooksByUser(int userId, int pageNumber);
    }
}
