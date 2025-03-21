using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.Repositories.ReadBooks;

namespace OnlineLibrary.Models.ReadBooks
{
    public class ReadBooksService : IReadBooksService
    {
        private IReadBooksRepository _readBooksRepository;

        public ReadBooksService(IReadBooksRepository readBooksRepository)
        {
            _readBooksRepository = readBooksRepository;
        }

        public async Task<ReadBooksModel> AddToReadBooks(int userId, ReadBooksDto readBooksDto)
        {
            var readBooks = await _readBooksRepository.GetAllReadBooksByUser(userId);
            if (readBooks == null) return null;

            if (readBooks.Books == null) readBooks.Books = new List<BookModel>();

            foreach (var book in readBooksDto.Books)
            {
                if (readBooks.Books.Contains(book))
                    readBooks.Books.Remove(book);

                readBooks.Books.Add(book);
            }

            return readBooks;
        }

        public async Task<ReadBooksModel> RemoveFromReadBooks(int userId, int bookId)
        {
            var readBooks = await _readBooksRepository.GetAllReadBooksByUser(userId);
            if (readBooks == null) return null;
            if (readBooks.Books == null) return null;

            foreach (var book in readBooks.Books)
            {
                if (book.Id == bookId)
                {
                    readBooks.Books.Remove(book);
                    break;
                }
            }
            
            return readBooks;
        }

        public async Task<bool> IsBookMarkedAsRead(int userId, int bookId)
        {
            var readBooks = await _readBooksRepository.GetAllReadBooksByUser(userId);
            if (readBooks == null) return false;

            foreach (var book in readBooks.Books)
            {
                if (book.Id != bookId) continue;
                return true;
            }

            return false;
        }
    }
}
