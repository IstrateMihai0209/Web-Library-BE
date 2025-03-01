namespace OnlineLibrary.Models.ReadBooks
{
    public interface IReadBooksService
    {
        Task<ReadBooksModel> UpdateReadBooksByUser(int userId, ReadBooksDto readBooksDto);
    }
}
