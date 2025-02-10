using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.Repositories.Book
{
    public class BookRepository : Repository<BookModel>, IBookRepository
    {
        public BookRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync(int userId)
        {
            return await _dbSet
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        public Task<IEnumerable<BookModel>> GetBooksByGenreAsync()
        {
            throw new NotImplementedException();
        }
    }
}
