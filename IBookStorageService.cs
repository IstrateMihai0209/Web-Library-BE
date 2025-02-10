using OnlineLibrary.Models.Book;

namespace OnlineLibrary
{
    public interface IBookStorageService
    {
        public Task UploadBook();

        public Task OpenBook(BookModel book);
    }
}
