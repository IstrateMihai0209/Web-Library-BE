
using Microsoft.EntityFrameworkCore;

namespace OnlineLibrary.Models.Repositories.Wishlist
{
    public class WishlistRepository : Repository<WishlistModel>, IWishlistRepository
    {
        public WishlistRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public async Task<WishlistModel> GetUserWishlistAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
