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

        public async Task<ReadBooksModel> UpdateReadBooksByUser(int userId, ReadBooksDto readBooksDto)
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
    }
}
