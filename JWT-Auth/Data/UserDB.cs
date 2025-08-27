using JWT_Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWT_Auth.Data
{
    public class UserDB(DbContextOptions<UserDB> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
