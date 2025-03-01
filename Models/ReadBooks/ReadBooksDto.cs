using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Models.ReadBooks
{
    public class ReadBooksDto
    {
        public ICollection<BookModel> Books { get; set; }
    }
}
