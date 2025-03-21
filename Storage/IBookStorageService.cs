using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Storage
{
    public interface IBookStorageService
    {
        public Task UploadBook(IFormFile coverImage, IFormFile textFile);

        public Task OpenBook(BookModel book);

        public string GetBookCoverUrl(BookModel book);
    }
}
