using DatingAPI.Models.DTOs;

namespace DatingAPI.Interfaces
{
    public interface IUserService
    {
        Task Registrate(UserDTO user);
        bool ComparePasswords(string enteredPassword, string savedPassword, byte[] salt);
    }
}
