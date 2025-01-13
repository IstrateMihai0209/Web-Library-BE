namespace OnlineLibrary.Models.Repositories.Book
{
    public class BookRepository : Repository<BookModel>, IBookRepository
    {
        public BookRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookModel>> GetBooksByCategoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookModel>> GetBooksByGenreAsync()
        {
            throw new NotImplementedException();
        }
    }
}
