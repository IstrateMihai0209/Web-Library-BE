
using Microsoft.EntityFrameworkCore;

namespace OnlineLibrary.Models.Repositories.ReadBooks
{
    public class ReadBooksRepository : Repository<ReadBooksModel>, IReadBooksRepository
    {
        public ReadBooksRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<ReadBooksModel> GetReadBooksByUser(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
