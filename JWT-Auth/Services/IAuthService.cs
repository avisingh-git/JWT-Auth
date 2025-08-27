using JWT_Auth.Entities;
using JWT_Auth.Entities.Models;

namespace JWT_Auth.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDTO req);
        Task<string?> LoginAsync(UserDTO req);

    }
}
