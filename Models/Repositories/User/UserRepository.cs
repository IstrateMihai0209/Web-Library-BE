namespace OnlineLibrary.Models.Repositories.User
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {
        public UserRepository(LibraryDbContext dbContext) : base(dbContext) { }
    }
}
