using OnlineLibrary.Models.Book;

namespace OnlineLibrary.Storage
{
    public class BookStorageService : IBookStorageService
    {
        private readonly IBookService _bookService;
        private readonly IFileStorageService _fileStorageService;

        public BookStorageService(IBookService bookService, IFileStorageService fileStorageService)
        {
            _bookService = bookService;
            _fileStorageService = fileStorageService;
        }

        public string GetBookCoverUrl(BookModel book)
        {
            var file = new JpgFile()
            {
                Name = _bookService.GetBookCoverNameFromStorage(book.Title),
                Path = book.CoverImage
            };

            return _fileStorageService.GetUrl(file);
        }

        public Task OpenBook(BookModel book)
        {
            throw new NotImplementedException();
        }

        public async Task UploadBook(IFormFile coverImage, IFormFile textFile)
        {
            await _fileStorageService.UploadFileAsync(coverImage, true);
            await _fileStorageService.UploadFileAsync(textFile, true);
        }
    }
}
