namespace OnlineLibrary.Models.Repositories.ReadBooks
{
    public interface IReadBooksRepository : IRepository<ReadBooksModel>
    {
        Task<ReadBooksModel> GetReadBooksByUser(int userId);
    }
}
