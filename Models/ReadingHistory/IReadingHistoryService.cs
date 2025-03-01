namespace OnlineLibrary.Models.ReadingHistory
{
    public interface IReadingHistoryService
    {
        Task<ReadingHistoryModel> UpdateReadingHistory(int userId, ReadingHistoryDto readingHistoryDto);
    }
}
