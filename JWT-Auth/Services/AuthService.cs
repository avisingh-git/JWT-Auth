using JWT_Auth.Data;
using JWT_Auth.Entities;
using JWT_Auth.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Auth.Services
{
    public class AuthService(UserDB context, IConfiguration configuration) : IAuthService
    {
        public async Task<string?> LoginAsync(UserDTO req)
        {
            var user = context.Users
                .FirstOrDefault(u => u.UserName == req.UserName);

            if (user == null)
            {
                return null;
            }

            if (user.UserName != req.UserName)
            {
                return null;
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, req.PasswordPlain)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return CreateToken(user);
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescription = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescription);

        }
        public async Task<User?> RegisterAsync(UserDTO req)
        {
            if(await context.Users.AnyAsync(u=>u.UserName == req.UserName))
            {
                return null;
            }
            var user = new User();
            var hashedPass = new PasswordHasher<User>()
                .HashPassword(user, req.PasswordPlain);

            user.UserName = req.UserName;
            user.PasswordHash = hashedPass;
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}
