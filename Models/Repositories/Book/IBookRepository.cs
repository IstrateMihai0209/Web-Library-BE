using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.Repositories.Book
{
    public interface IBookRepository : IRepository<BookModel>
    {
        Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync(int userId);

        Task<IEnumerable<BookModel>> GetBooksByCategoryAsync(int categoryId);

        Task<IEnumerable<BookModel>> GetBooksByGenreAsync();
    }
}
