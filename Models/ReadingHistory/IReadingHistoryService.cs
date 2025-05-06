namespace OnlineLibrary.Models.ReadingHistory
{
    public interface IReadingHistoryService
    {
        Task<ReadingHistoryModel> UpdateReadingHistory(string userId, ReadingHistoryDto readingHistoryDto);
    }
}
