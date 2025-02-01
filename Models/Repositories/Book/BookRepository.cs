namespace OnlineLibrary.Models.Repositories.Book
{
    public class BookRepository : Repository<BookModel>, IBookRepository
    {
        public BookRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookModel>> GetBooksByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookModel>> GetBooksByGenreAsync()
        {
            throw new NotImplementedException();
        }
    }
}
