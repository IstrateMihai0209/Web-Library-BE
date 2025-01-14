namespace OnlineLibrary.Models.Repositories.Role
{
    public interface IRoleRepository : IRepository<RoleModel>
    {
        Task<RoleModel> GetRoleOfUserAsync();
    }
}
