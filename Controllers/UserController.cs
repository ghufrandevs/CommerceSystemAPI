using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
using CommerceSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController: ControllerBase
    {
        //inject service in constructor
        private readonly AppDbContext _context;

        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        private readonly UserService _userService;
        public UserController(AppDbContext context, PasswordService passwordService, JwtService jwtService, EmailService emailService, UserService userService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _emailService = emailService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserRegisterDTO dto)
        {
            string result = _userService.Register(dto);

            if (result != "Account Created Successfully")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginDTO dto)
        {
            var result = _userService.Login(dto);

            if (result is string)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers() 
        {
           var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(user);
            }

           [Authorize]
           [HttpPut("UpdateUser")]
            public IActionResult UpdateUser(int id, UserUpdateDTO dto)
            {
            int loggedInUserId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            bool isAdmin = User.IsInRole("Admin");

            string result = _userService.UpdateUser(id,dto, loggedInUserId, isAdmin);
              
            if (result == "User Not Found")
            {
                return NotFound(result);
            }

            if (result == "Forbidden")
            {
                return Forbid();
            }

            return Ok(result);
        }



    }

}
