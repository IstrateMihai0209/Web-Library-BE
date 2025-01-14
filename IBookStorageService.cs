using OnlineLibrary.Models;

namespace OnlineLibrary
{
    public interface IBookStorageService
    {
        public Task UploadBook();

        public Task OpenBook(BookModel book);
    }
}
