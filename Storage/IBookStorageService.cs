using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Storage
{
    public interface IBookStorageService
    {
        public Task UploadBook();

        public Task OpenBook(BookModel book);

        public string GetBookCoverUrl(BookModel book);
    }
}
