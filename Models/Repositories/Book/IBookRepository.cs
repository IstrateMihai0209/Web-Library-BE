using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.Repositories.Book
{
    public interface IBookRepository : IRepository<BookModel>
    {
        Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync(string userId, int pageNumber);

        Task<IEnumerable<BookModel>> GetTopPopularBooksAsync(int pageNumber);

        Task<IEnumerable<BookModel>> GetSimilarBooksAsync(BookModel bookModel);

        Task<IEnumerable<BookModel>> SearchBooks(string searchQuery, Dictionary<string, List<string>> filters, int pageNumber);
    }
}
