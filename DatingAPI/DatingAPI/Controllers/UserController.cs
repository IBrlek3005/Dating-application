using DatingAPI.Data;
using DatingAPI.Helpers;
using DatingAPI.Interfaces;
using DatingAPI.Models;
using DatingAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DatingContext _context;
        private readonly IUserService _userService;
        private readonly IJwtHelper _jwtHelper;
        public UserController(IUserService userService, DatingContext context, IConfiguration configuration, IJwtHelper jwtHelper)
        {
            _userService = userService;
            _context = context;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("registrate")]
        public async Task<IActionResult> Registreate([FromBody]UserDTO request)
        {
            await _userService.Registrate(request);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginDTO loginDTO)
        {
            if(loginDTO.Username == null)
            {
                return BadRequest("Missing username.");
            }

            if (loginDTO.Password == null)
            {
                return BadRequest("Missing password.");
            }
            var user = _context.GetUserByUsername(loginDTO.Username);

            if (user == null)
            {
                return BadRequest("There is no user with that username.");
            }

            if(! _userService.ComparePasswords(loginDTO.Password, user.Password!, user.Salt!))
            {
                return BadRequest("The password is incorrect");
            }

            if (!Int32.TryParse(_configuration["Authentication: TokenDuration"], out var tokenDuration))
            {
                tokenDuration = 300;
            }

            var now = DateTime.Now;

            var expirationTime = now.AddSeconds(tokenDuration).AddMinutes(30);

            string refreshToken = _jwtHelper.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Created = now,
                ExpiresOn = expirationTime,
                SessionId = Guid.NewGuid().ToString(),
                TokenString = refreshToken,
                UserId = user.Id
            };

            _context.RefreshToken.Add(refreshTokenEntity);
            _context.SaveChanges();

            var token = _jwtHelper.GenerateToken(
                user.Id.ToString(),
                user.UserName,
                //null,
                refreshTokenEntity.SessionId,
                tokenDuration,
                now
                );

            return new LoginResponse
            {
                RefreshToken = refreshToken,
                Token = token,
                User = new User
                {
                    Id = user.Id,
                    UserName = user.UserName
                }
            };
        }
    }
}
