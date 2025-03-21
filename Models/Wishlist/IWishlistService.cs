namespace OnlineLibrary.Models.Wishlist
{
    public interface IWishlistService
    {
        Task<WishlistModel> AddToWishlist(int userId, WishlistDto wishlistDto);
        
        Task<WishlistModel> RemoveFromWishlist(int userId, int bookId);

        Task<bool> IsBookInWishlist(int userId, int bookId);
    }
}
