using Extensions;
using OnlineLibrary.Models.Repositories.Book;

namespace OnlineLibrary.Models.Book
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public BookModel CreateBookModel(BookDto bookDto)
        {
            var path = $"https://weblibrary.blob.core.windows.net/lib-files";

            var bookModel = new BookModel()
            {
               Title = bookDto.Title,
               Author = bookDto.Author,
               Publisher = bookDto.Publisher,
               Genre = bookDto.Genre,
               Description = bookDto.Description,
               MoreAboutAuthor = bookDto.MoreAboutAuthor,
               PublishDate = bookDto.PublishDate,
               CategoryId = 1, //TODO: Determine the category based on some calculations
               FilePath = $"{path}/{bookDto.TextFile.FileName}{bookDto.TextFile.GetFileExtension()}", 
               UploadedAt = DateTime.Now,
               UserId = bookDto.UploaderId,
               CoverImage = $"{path}/{bookDto.CoverImage.FileName}{bookDto.CoverImage.GetFileExtension()}", 
            };

            return bookModel;
        }

        public async Task<BookModel> UpdateBook(int id, BookDto bookDto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return null;

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Publisher = bookDto.Publisher;
            book.Genre = bookDto.Genre;
            book.Description = bookDto.Description;
            //book.FilePath = ""; //TODO: Determine the file path after the pdf is loaded in Azure
            book.PublishDate = bookDto.PublishDate;
            //book.CoverImage = ""; //TODO: Determine the file path after the image is loaded in Azure

            return book;
        }

        public void IncreaseBookPopularity(BookModel book)
        {
            book.Popularity++;
        }

        public string GetBookCoverNameFromStorage(string name)
        {
            var newName = name.ToLower().Replace(' ', '-');
            return newName;
        }
    }
}
