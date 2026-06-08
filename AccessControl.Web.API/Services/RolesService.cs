using AccessControl.Web.API.DBConfiguration;
using AccessControl.Web.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Web.API.Services
{
    public class RolesService : IRoleService
    {
        private readonly ApplicationDbContext _dbContext;
        public RolesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Roles> CreateRolesAsync(Roles roles)
        {
            try
            {
                if (roles == null)
                {
                    throw new ArgumentNullException(nameof(roles), "Role object cannot be null.");
                }
                await _dbContext.Roles.AddAsync(roles);
                await _dbContext.SaveChangesAsync();
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating role: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteRolesAsync(int id)
        {
            try
            {
                var roles = _dbContext.Roles.FirstOrDefault(u => u.RoleId == id);
                if (roles == null)
                {
                    return false;
                }
                _dbContext.Roles.Remove(roles);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Roles>> GetAllRolesAsync()
        {
            try
            {
                return await _dbContext.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving roles: {ex.Message}", ex);
            }
        }

        public async Task<Roles> GetRolesByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid Role id :", nameof(id));
                }
                return await _dbContext.Roles.FirstOrDefaultAsync(u => u.RoleId == id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Roles> UpdateRolesAsync(int id, Roles roles)
        {
            try
            {
                if (roles == null)
                {
                    return null;
                }
                var DBContext = _dbContext.Roles.FirstOrDefault(u => u.RoleId == id);
                if (DBContext == null)
                {
                    return null;
                }
                if (DBContext != null)
                {
                   
                    DBContext.RoleName = roles.RoleName;
                    DBContext.Description = roles.Description;
                    DBContext.IsActive = roles.IsActive;
                    DBContext.CreatedBy = roles.CreatedBy;
                    DBContext.CreatedDate = roles.CreatedDate;
                    DBContext.ModifiedBy = roles.ModifiedBy;
                    DBContext.ModifiedDate = roles.ModifiedDate;

                }
                await _dbContext.SaveChangesAsync();
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
