using OnlineLibrary.Models;

namespace OnlineLibrary
{
    public class BookStorageService : IBookStorageService
    {
        private readonly IFileStorageService _fileStorageService;

        public BookStorageService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public Task OpenBook(BookModel book)
        {
            throw new NotImplementedException();
        }

        public Task UploadBook()
        {
            throw new NotImplementedException();
        }
    }
}
