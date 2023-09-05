using DatingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data
{
    public class DatingContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserToken> Token { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DatingContext()
        {

        }

        public DatingContext(DbContextOptions<DatingContext> options): base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=DatingApp;Integrated Security=True;TrustServerCertificate=True;");
        }

        public User GetUserByUsername(string username)
        {
            return User.SingleOrDefault(x => x.UserName == username);
        }
    }
}
