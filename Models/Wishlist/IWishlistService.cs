namespace OnlineLibrary.Models.Wishlist
{
    public interface IWishlistService
    {
        Task<WishlistModel> UpdateWishlist(int userId, WishlistDto wishlistDto);
    }
}
