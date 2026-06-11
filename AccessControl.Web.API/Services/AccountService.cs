using AccessControl.Web.API.DBConfiguration;
using AccessControl.Web.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccessControl.Web.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly string _tokenKey;

        public AccountService(ILogger<AccountService> logger, ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _tokenKey = configuration.GetValue<string>("TokenKey");
        }
        public async Task<AuthenticationResponse> AuthenticateAsync(UserLogin userLogin)
        {
            _logger.LogInformation("Authenticating user {Username}", userLogin.Username);
            try
            {
                var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == userLogin.Username.Trim().ToLower() || u.Phone.Trim() == userLogin.Username.Trim());

                if (user == null)
                {
                    return new AuthenticationResponse
                    {
                        IsValidUser = false,
                        IsValidPassword = false,
                        Message = $"User not found with given credentials {userLogin.Username}"
                    };
                }
                else
                {
                    if (user.Password.Trim().ToLower() == userLogin.Password.Trim().ToLower())
                    {
                        if (user.IsActive)
                        {
                            //genarate jwt token and refresh token here

                            var tokenHandler = new JwtSecurityTokenHandler();

                            var tokenKey = Encoding.ASCII.GetBytes(_tokenKey);

                            var tokenDescrptor = new SecurityTokenDescriptor
                            {
                                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                                          {
                                                new Claim(ClaimTypes.Name,userLogin.Username),
                                                new Claim("UserId",user.Id.ToString()),
                                                new Claim(ClaimTypes.Role,user.Role)
                                          }),
                                Expires = DateTime.UtcNow.AddMinutes(30),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                            };

                            var token = tokenHandler.CreateToken(tokenDescrptor);

                            var writeToken = tokenHandler.WriteToken(token);


                            return new AuthenticationResponse
                            {
                                IsValidUser = true,
                                IsValidPassword = true,
                                JwtToken = writeToken,
                                Message = $"User authenticated successfully {userLogin.Username}"
                            };
                        }
                        else
                        {
                            return new AuthenticationResponse
                            {
                                IsValidUser = true,
                                IsValidPassword = true,
                                Message = $"User account is inactive {userLogin.Username}"
                            };
                        }

                    }
                    else
                    {
                        return new AuthenticationResponse
                        {
                            IsValidUser = true,
                            IsValidPassword = false,
                            Message = "Invalid password"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error authenticating user: {ex.Message}");
                return new AuthenticationResponse
                {
                    IsValidUser = false,
                    IsValidPassword = false,
                    Message = $"Error authenticating user: {ex.Message}"
                };
            }
        }
    }
}
