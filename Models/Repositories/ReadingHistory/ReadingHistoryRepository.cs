using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.ReadingHistory;

namespace OnlineLibrary.Models.Repositories.ReadingHistory
{
    public class ReadingHistoryRepository : Repository<ReadingHistoryModel>, IReadingHistoryRepository, IReturnsBookList
    {
        public int Count => 40;

        private readonly LibraryDbContext _dbContext;
        
        public ReadingHistoryRepository(LibraryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReadingHistoryModel> GetOrCreateUserReadingHistoryWithAllBooksAsync(string userId)
        {
            await CreateReadingHistoryIfNeeded(userId);
            
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(rh => rh.Books)
                .FirstOrDefaultAsync();
        }

        public async Task<ReadingHistoryModel> GetReadingHistoryOfUserWithoutBooksAsync(string userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<ReadingHistoryModel> GetOrCreateReadingHistoryOfUserAsync(string userId, int pageNumber)
        {
            await CreateReadingHistoryIfNeeded(userId);
            
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(rh => rh.Books.Skip(skip).Take(Count))
                .FirstOrDefaultAsync();
        }
        
        private async Task CreateReadingHistoryIfNeeded(string userId)
        {
            var existingReadingHistory = await GetReadingHistoryOfUserWithoutBooksAsync(userId);
            if (existingReadingHistory == null)
            {
                var newReadingHistory = new ReadingHistoryModel()
                {
                    UserId = userId,
                    Books = new List<BookModel>()
                };
                
                await AddAsync(newReadingHistory);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
