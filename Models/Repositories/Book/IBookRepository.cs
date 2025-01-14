namespace OnlineLibrary.Models.Repositories.Book
{
    public interface IBookRepository : IRepository<BookModel>
    {
        Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync();

        Task<IEnumerable<BookModel>> GetBooksByCategoryAsync();

        Task<IEnumerable<BookModel>> GetBooksByGenreAsync();
    }
}
