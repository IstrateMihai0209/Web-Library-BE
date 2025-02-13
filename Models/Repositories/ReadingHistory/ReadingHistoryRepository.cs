using Microsoft.EntityFrameworkCore;

namespace OnlineLibrary.Models.Repositories.ReadingHistory
{
    public class ReadingHistoryRepository : Repository<ReadingHistoryModel>, IReadingHistoryRepository
    {
        public ReadingHistoryRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<ReadingHistoryModel> GetReadingHistoryOfUserAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
