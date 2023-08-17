using GenerateTokenDemo.Models;
using GenerateTokenDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenerateTokenDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Tokeniser _tokeniser;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokeniser = new Tokeniser(_configuration);
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
            };

            UsersStore.AddNewUser(newUser);

            return Ok(newUser);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            var user = UsersStore.GetAllUsers().FirstOrDefault(u => u.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest("Wrong Username or Password.");
            }
            
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong Username or Password");
            }

            string token = _tokeniser.GenerateToken(user);

            return Ok(token);
        }
    }
}
