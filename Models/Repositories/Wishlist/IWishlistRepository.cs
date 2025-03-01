using OnlineLibrary.Models.Wishlist;

namespace OnlineLibrary.Models.Repositories.Wishlist
{
    public interface IWishlistRepository : IRepository<WishlistModel>
    {
        Task<WishlistModel> GetUserWishlistWithNoBooksAsync(int userId);

        Task<WishlistModel> GetUserWishlistWithAllBooksAsync(int userId);

        Task<WishlistModel> GetUserWishlistAsync(int userId, int pageNumber);
    }
}
