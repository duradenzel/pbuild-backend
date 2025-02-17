// pbuild-api/Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using pbuild_business.Services;
using pbuild_domain.Entities;

namespace pbuild_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            
            if (user == null)
            {
                return NotFound(); 
            }

            return Ok(user); 
        }
    }
}
