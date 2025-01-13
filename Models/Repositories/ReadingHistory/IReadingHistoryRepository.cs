namespace OnlineLibrary.Models.Repositories.ReadingHistory
{
    public interface IReadingHistoryRepository : IRepository<ReadingHistoryModel>
    {
        Task<ReadingHistoryModel> GetReadingHistoryOfUserAsync();
    }
}
