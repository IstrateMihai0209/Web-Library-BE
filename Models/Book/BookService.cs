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

        public async Task<BookModel> UpdateBook(int id, BookDto bookDto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return null;

            book.Title = bookDto.Title;
            book.Genre = bookDto.Genre;
            book.Description = bookDto.Description;
            book.FilePath = bookDto.FilePath;
            book.PublishDate = bookDto.PublishDate;
            book.UploadedAt = bookDto.UploadedAt;
            book.CoverImage = bookDto.CoverImage;
            book.Popularity = bookDto.Popularity;

            return book;
        }

        public void IncreaseBookPopularity(BookModel book)
        {
            book.Popularity++;
        }
    }
}
