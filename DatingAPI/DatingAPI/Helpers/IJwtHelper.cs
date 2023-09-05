using System.Security.Claims;

namespace DatingAPI.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(
            string userId,
            string username,
            //List<string> roles,
            string sessionId,
            long sessionDuretion,
            DateTime sessionStart);
        string GenerateRefreshToken();
    }
}
