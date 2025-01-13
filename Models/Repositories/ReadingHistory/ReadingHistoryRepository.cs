namespace OnlineLibrary.Models.Repositories.ReadingHistory
{
    public class ReadingHistoryRepository : Repository<ReadingHistoryModel>, IReadingHistoryRepository
    {
        public ReadingHistoryRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public Task<ReadingHistoryModel> GetReadingHistoryOfUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
