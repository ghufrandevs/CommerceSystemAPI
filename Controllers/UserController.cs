using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
using CommerceSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        public UserController(AppDbContext context, PasswordService passwordService, JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserRegisterDTO dto)
        {
            if (_context.Users.Any(u => u.UserEmail == dto.UserEmail))
            {
                return BadRequest("This Email Already Exists");
            }

            User user = new User()
            {
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                UserPassword = _passwordService.HashPassword(dto.UserPassword),
                UserPhone = dto.UserPhone,
                Role = "Customer",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Account Created Successfully");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = _context.Users
    .FirstOrDefault(u =>
        u.UserEmail == dto.UserEmail);

            if (user == null)
            {
                return BadRequest("Invalid Email Or Password");
            }
            
            bool isValidPassword =
           _passwordService.VerifyPassword(
           dto.UserPassword,
           user.UserPassword);

            if (!isValidPassword)
            {
                return BadRequest("Invalid Email Or Password");
            }

            if (!user.IsActive)
            {
                return BadRequest("Your account is inactive");
            }
            string token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Message = "Login Successful",
                Token = token,
                UserId = user.UserId,
                Role = user.Role
            });
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(User user) 
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User Added Successfully");

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers() 
        {
            var usersDto = _context.Users
          .Select(u => new UserOutputDTO
        {
        UserId = u.UserId,
        UserName = u.UserName,
        UserEmail = u.UserEmail,
        UserPhone = u.UserPhone
       })
        .ToList();

            return Ok(usersDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id) 
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            UserOutputDTO userOutput = new UserOutputDTO()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserPhone = user.UserPhone
            };

            return Ok(userOutput);
        }
        [Authorize]
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(int id, User user)
        {
            var usr = _context.Users.Find(id);

            if (usr == null)
            {
                return NotFound("User Not Found");
            }

            usr.UserName = user.UserName;
            usr.UserEmail = user.UserEmail;
            usr.UserPassword = user.UserPassword;
            usr.UserPhone = user.UserPhone;
            usr.Role = user.Role;
            usr.IsActive = user.IsActive;

            _context.Users.Update(usr);
            _context.SaveChanges();

            return Ok("User Updated Successfully");
        }





        }

}
