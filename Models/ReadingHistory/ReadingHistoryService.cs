
using OnlineLibrary.Models.Book;
using OnlineLibrary.Models.Repositories.ReadingHistory;

namespace OnlineLibrary.Models.ReadingHistory
{
    public class ReadingHistoryService : IReadingHistoryService
    {
        private IReadingHistoryRepository _readingHistoryRepository;

        public ReadingHistoryService(IReadingHistoryRepository readingHistoryRepository)
        {
            _readingHistoryRepository = readingHistoryRepository;
        }

        public async Task<ReadingHistoryModel> UpdateReadingHistory(string userId, ReadingHistoryDto readingHistoryDto)
        {
            var readingHistory = await _readingHistoryRepository.GetOrCreateUserReadingHistoryWithAllBooksAsync(userId);
            if (readingHistory == null) return null;

            if (readingHistory.Books == null) readingHistory.Books = new List<BookModel>();

            foreach(var book in readingHistoryDto.Books)
            {
                if(readingHistory.Books.Contains(book))
                    readingHistory.Books.Remove(book);

                readingHistory.Books.Add(book);
            }

            readingHistory.AccessDate = DateTime.Now;

            return readingHistory;
        }
    }
}
