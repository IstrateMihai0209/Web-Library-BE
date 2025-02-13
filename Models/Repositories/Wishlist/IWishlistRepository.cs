namespace OnlineLibrary.Models.Repositories.Wishlist
{
    public interface IWishlistRepository : IRepository<WishlistModel>
    {
        Task<WishlistModel> GetUserWishlistAsync(int userId);
    }
}
