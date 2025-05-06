namespace OnlineLibrary.Models.ReadBooks
{
    public interface IReadBooksService
    {
        Task<ReadBooksModel> AddToReadBooks(string userId, ReadBooksDto readBooksDto);

        Task<ReadBooksModel> RemoveFromReadBooks(string userId, int bookId);

        public Task<bool> IsBookMarkedAsRead(string userId, int bookId);
    }
}
