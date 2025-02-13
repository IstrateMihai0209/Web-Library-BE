using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.Repositories.Book
{
    public class BookRepository : Repository<BookModel>, IBookRepository
    {
        private const int DefaultBooksPerPage = 40;

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

        public async Task<IEnumerable<BookModel>> GetTopPopularBooksAsync(int count, int pageNumber)
        {
            var skip = (pageNumber - 1) * count;

            return await _dbSet
                .OrderByDescending(b => b.Popularity)
                .Skip(skip)
                .Take(count)
                .ToListAsync();
        }
    }
}
