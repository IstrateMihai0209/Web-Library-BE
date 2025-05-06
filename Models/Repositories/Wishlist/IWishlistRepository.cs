using OnlineLibrary.Models.Wishlist;

namespace OnlineLibrary.Models.Repositories.Wishlist
{
    public interface IWishlistRepository : IRepository<WishlistModel>
    {
        Task<WishlistModel> GetUserWishlistWithNoBooksAsync(string userId);

        Task<WishlistModel> GetOrCreateUserWishlistWithAllBooksAsync(string userId);

        Task<WishlistModel> GetOrCreateUserWishlistAsync(string userId, int pageNumber);
    }
}
