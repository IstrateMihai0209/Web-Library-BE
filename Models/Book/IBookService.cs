namespace OnlineLibrary.Models.Book
{
    public interface IBookService
    {
        Task<BookModel> UpdateBook(int id, BookDto bookDto);
    }
}
