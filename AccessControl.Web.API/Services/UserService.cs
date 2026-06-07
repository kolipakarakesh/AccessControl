using AccessControl.Web.API.DBConfiguration;
using AccessControl.Web.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Web.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                await _dbContext.users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            //Delete from User Where Id=1
            //select * from User where Id=1
            try
            {
                var user = _dbContext.users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return false;

                }
                _dbContext.users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _dbContext.users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid user ID.", nameof(id));
                }
                return await _dbContext.users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            try
            {
                if (user ==null)
                {
                    return null;
                }

                var DBUser = _dbContext.users.FirstOrDefault(u => u.Id == id);
                if (DBUser == null)
                {
                    return null;
                }

                if (DBUser != null)
                {
                    DBUser.Name = user.Name;
                    DBUser.Email = user.Email;
                    DBUser.Password = user.Password;
                    DBUser.Phone = user.Phone;
                    DBUser.Role = user.Role;
                    DBUser.IsBlocked = user.IsBlocked;
                    DBUser.IsActive = user.IsActive;

                }
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}