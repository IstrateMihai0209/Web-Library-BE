using OnlineLibrary.Models.ReadingHistory;

namespace OnlineLibrary.Models.Repositories.ReadingHistory
{
    public interface IReadingHistoryRepository : IRepository<ReadingHistoryModel>
    {
        Task<ReadingHistoryModel> GetUserReadingHistoryWithAllBooksAsync(int userId);

        Task<ReadingHistoryModel> GetReadingHistoryOfUserWithoutBooksAsync(int userId);

        Task<ReadingHistoryModel> GetReadingHistoryOfUserAsync(int userId, int pageNubmer);
    }
}
