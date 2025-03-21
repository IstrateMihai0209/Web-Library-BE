using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.ReadingHistory;

namespace OnlineLibrary.Models.Repositories.ReadingHistory
{
    public class ReadingHistoryRepository : Repository<ReadingHistoryModel>, IReadingHistoryRepository, IReturnsBookList
    {
        public int Count => 40;

        public ReadingHistoryRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<ReadingHistoryModel> GetUserReadingHistoryWithAllBooksAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(rh => rh.Books)
                .FirstOrDefaultAsync();
        }

        public async Task<ReadingHistoryModel> GetReadingHistoryOfUserWithoutBooksAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<ReadingHistoryModel> GetReadingHistoryOfUserAsync(int userId, int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(rh => rh.Books.Skip(skip).Take(Count))
                .FirstOrDefaultAsync();
        }
    }
}
