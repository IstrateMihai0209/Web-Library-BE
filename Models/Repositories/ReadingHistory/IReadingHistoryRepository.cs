using OnlineLibrary.Models.ReadingHistory;

namespace OnlineLibrary.Models.Repositories.ReadingHistory
{
    public interface IReadingHistoryRepository : IRepository<ReadingHistoryModel>
    {
        Task<ReadingHistoryModel> GetOrCreateUserReadingHistoryWithAllBooksAsync(string userId);

        Task<ReadingHistoryModel> GetReadingHistoryOfUserWithoutBooksAsync(string userId);

        Task<ReadingHistoryModel> GetOrCreateReadingHistoryOfUserAsync(string userId, int pageNubmer);
    }
}
