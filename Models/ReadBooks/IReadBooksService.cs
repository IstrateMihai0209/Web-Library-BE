namespace OnlineLibrary.Models.ReadBooks
{
    public interface IReadBooksService
    {
        Task<ReadBooksModel> AddToReadBooks(int userId, ReadBooksDto readBooksDto);

        Task<ReadBooksModel> RemoveFromReadBooks(int userId, int bookId);

        public Task<bool> IsBookMarkedAsRead(int userId, int bookId);
    }
}
