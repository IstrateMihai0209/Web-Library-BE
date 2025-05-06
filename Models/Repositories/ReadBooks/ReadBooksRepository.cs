
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.ReadBooks;

namespace OnlineLibrary.Models.Repositories.ReadBooks
{
    public class ReadBooksRepository : Repository<ReadBooksModel>, IReadBooksRepository, IReturnsBookList
    {
        public int Count => 40;

        private readonly LibraryDbContext _dbContext;
        
        public ReadBooksRepository(LibraryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReadBooksModel> GetOrCreateReadBooksByUser(string userId, int pageNumber)
        {
            await CreateReadBooksSectionIfNeeded(userId);
            
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books.Skip(skip).Take(Count))
                .FirstOrDefaultAsync();
        }

        public async Task<ReadBooksModel> GetOrCreateAllReadBooksByUser(string userId)
        {
            await CreateReadBooksSectionIfNeeded(userId);
            
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books)
                .FirstOrDefaultAsync();
        }
        
        public async Task<ReadBooksModel> GetReadBooksSectionByUser(string userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
        
        private async Task CreateReadBooksSectionIfNeeded(string userId)
        {
            var existingReadBooksSection = await GetReadBooksSectionByUser(userId);
            if (existingReadBooksSection == null)
            {
                var newReadBooksSection = new ReadBooksModel()
                {
                    UserId = userId,
                    Books = new List<BookModel>()
                };
                
                await AddAsync(newReadBooksSection);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
