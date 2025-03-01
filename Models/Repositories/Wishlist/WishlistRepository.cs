
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Wishlist;

namespace OnlineLibrary.Models.Repositories.Wishlist
{
    public class WishlistRepository : Repository<WishlistModel>, IWishlistRepository, IReturnsBookList
    {
        public int Count => 40;

        public WishlistRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<WishlistModel> GetUserWishlistAsync(int userId, int pageNumber)
        {
            var skip = (pageNumber - 1) * Count;

            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books.Skip(skip).Take(Count))
                .FirstOrDefaultAsync();
        }

        public async Task<WishlistModel> GetUserWishlistWithAllBooksAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Include(list => list.Books)
                .FirstOrDefaultAsync();
        }

        public async Task<WishlistModel> GetUserWishlistWithNoBooksAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
