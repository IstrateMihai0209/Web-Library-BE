using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.Repositories.Wishlist;

namespace OnlineLibrary.Models.Wishlist
{
    public class WishlistService : IWishlistService
    {
        private IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<WishlistModel> UpdateWishlist(int userId, WishlistDto wishlistDto)
        {
            var wishlist = await _wishlistRepository.GetUserWishlistWithAllBooksAsync(userId);
            if (wishlist == null) return null;

            if (wishlist.Books == null) wishlist.Books = new List<BookModel>();

            foreach (var book in wishlistDto.Books)
            {
                if (wishlist.Books.Contains(book))
                    wishlist.Books.Remove(book);

                wishlist.Books.Add(book);
                    
            }

            return wishlist;
        }
    }
}
