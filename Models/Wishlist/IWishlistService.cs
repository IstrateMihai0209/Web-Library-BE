namespace OnlineLibrary.Models.Wishlist
{
    public interface IWishlistService
    {
        Task<WishlistModel> AddToWishlist(string userId, WishlistDto wishlistDto);
        
        Task<WishlistModel> RemoveFromWishlist(string userId, int bookId);

        Task<bool> IsBookInWishlist(string userId, int bookId);
    }
}
