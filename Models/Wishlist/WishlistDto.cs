using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.Wishlist
{
    public class WishlistDto
    {
        public ICollection<BookModel> Books { get; set; }
    }
}
