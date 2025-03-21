namespace OnlineLibrary.Models.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        void Update(T entity);

        Task<bool> DeleteAsync(int id);

        void Detach(T entity);
    }
}
