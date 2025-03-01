using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.Repositories.Book
{
    public class BookRepository : Repository<BookModel>, IBookRepository, IReturnsBookList
    {
        public int Count => 40;

        public BookRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<BookModel>> GetBooksOfUploaderAsync(int userId, int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(b => b.UserId == userId)
                .Skip(skip)
                .Take(Count)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookModel>> GetTopPopularBooksAsync(int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .OrderByDescending(b => b.Popularity)
                .Skip(skip)
                .Take(Count)
                .ToListAsync();
        }
    }
}
