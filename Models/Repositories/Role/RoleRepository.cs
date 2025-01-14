
namespace OnlineLibrary.Models.Repositories.Role
{
    public class RoleRepository : Repository<RoleModel>, IRoleRepository
    {
        public RoleRepository(LibraryDbContext dbContext) : base(dbContext) { }

        public Task<RoleModel> GetRoleOfUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
