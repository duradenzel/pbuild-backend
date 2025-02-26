// pbuild-api/Controllers/UserController.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pbuild_business.Services;
using pbuild_domain.Entities;

namespace pbuild_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;


        public UserController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin");
            Console.WriteLine(hashedPassword);
            var user = _userService.GetUserById(id);
            
            if (user == null)
            {
                return NotFound(); 
            }

            return Ok(user); 
        }

        [HttpPost("login")]
         [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _authService.AuthenticateUser(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _authService.GenerateToken(user);
            return Ok(new { Token = token, UserId = user.Id, Email = user.Email });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var existingUser = _userService.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword,
                Role = "User" 
            };

            _authService.CreateUser(newUser);
            return Ok("User registered successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult GetAdminDashboard()
        {
            return Ok("admin auth successful");
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            return Ok(new { Message = "You are authenticated!", UserId = userId });
        }

        public class LoginRequest
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        public class RegisterRequest
        {
            public required string Name { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
        }
       
   
    
    }
}
