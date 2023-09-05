using DatingAPI.Data;
using DatingAPI.Exceptions;
using DatingAPI.Interfaces;
using DatingAPI.Models;
using DatingAPI.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace DatingAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DatingContext context = new DatingContext();

        public UserService(DatingContext _context)
        {
            context = _context;
        }
        public Task Registrate(UserDTO user)
        {
            
            if(user == null)
            {
                throw new NullExection("Nije unesen korisnik");
            }

            var postojeciUser = context.GetUserByUsername(user.UserName!);

            if(postojeciUser is not null)
            {
                throw new NullExection("Nije moguće unijeti korisnik. Korisnik");
            }

            var salt = GenerateSalt();

            var newUser = new User
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = Guid.NewGuid(),
                Password = ComputeHash(user.Password!, salt),
                Salt = salt,
                UserName = user.UserName
            };
            context.User.Add(newUser);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        static string ComputeHash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] saltedPasswordBytes = new byte[salt.Length + passwordBytes.Length];
            Buffer.BlockCopy(salt, 0, saltedPasswordBytes, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, salt.Length, passwordBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool ComparePasswords(string enteredPassword, string savedPassword, byte[] salt)
        {
            var hasedNew = ComputeHash(enteredPassword, salt);

            return hasedNew == savedPassword;
        }
    }
}
