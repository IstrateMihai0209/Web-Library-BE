namespace OnlineLibrary.Models.Book
{
    public interface IBookService
    {
        BookModel CreateBookModel(BookDto bookDto);

        string GetBookCoverNameFromStorage(string name);

        Task<BookModel> UpdateBook(int id, BookDto bookDto);
    
        void IncreaseBookPopularity(BookModel bookModel);
    }
}
