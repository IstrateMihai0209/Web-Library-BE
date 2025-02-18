namespace OnlineLibrary.Models.Book
{
    public interface IBookService
    {
        string GetBookCoverNameFromStorage(string name);

        Task<BookModel> UpdateBook(int id, BookDto bookDto);
    
        void IncreaseBookPopularity(BookModel bookModel);
    }
}
