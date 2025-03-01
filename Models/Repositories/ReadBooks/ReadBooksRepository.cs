
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.ReadBooks;

namespace OnlineLibrary.Models.Repositories.ReadBooks
{
    public class ReadBooksRepository : Repository<ReadBooksModel>, IReadBooksRepository, IReturnsBookList
    {
        public int Count => 40;

        public ReadBooksRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<ReadBooksModel> GetReadBooksByUser(int userId, int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books.Skip(skip).Take(Count))
                .FirstOrDefaultAsync();
        }

        public async Task<ReadBooksModel> GetAllReadBooksByUser(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books)
                .FirstOrDefaultAsync();
        }

        public async Task<ReadBooksModel> GetReadBooksSectionByUser(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
