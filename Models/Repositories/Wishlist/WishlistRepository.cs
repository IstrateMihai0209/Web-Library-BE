
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.Repositories.UnitOfWork;
using OnlineLibrary.Models.Wishlist;

namespace OnlineLibrary.Models.Repositories.Wishlist
{
    public class WishlistRepository : Repository<WishlistModel>, IWishlistRepository, IReturnsBookList
    {
        public int Count => 40;

        private readonly LibraryDbContext _dbContext;

        public WishlistRepository(LibraryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WishlistModel> GetOrCreateUserWishlistAsync(string userId, int pageNumber)
        {
            await CreateWishlistIfNeeded(userId);
            
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books.Skip(skip).Take(Count))
                .FirstOrDefaultAsync();
        }

        public async Task<WishlistModel> GetOrCreateUserWishlistWithAllBooksAsync(string userId)
        {
            await CreateWishlistIfNeeded(userId);
            
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books)
                .FirstOrDefaultAsync();
        }

        public async Task<WishlistModel> GetUserWishlistWithNoBooksAsync(string userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        private async Task CreateWishlistIfNeeded(string userId)
        {
            var existingWishlist = await GetUserWishlistWithNoBooksAsync(userId);
            if (existingWishlist == null)
            {
                var newWishlist = new WishlistModel
                {
                    UserId = userId,
                    Books = new List<BookModel>()
                };
                
                await AddAsync(newWishlist);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
