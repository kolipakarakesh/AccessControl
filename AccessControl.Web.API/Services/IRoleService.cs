using AccessControl.Web.API.Models;

namespace AccessControl.Web.API.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Roles>> GetAllRolesAsync();
        Task<Roles> GetRolesByIdAsync(int id);
        Task<Roles> CreateRolesAsync(Roles roles);
        Task<Roles> UpdateRolesAsync(int id, Roles roles);
        Task<bool> DeleteRolesAsync(int id);
    }
}
