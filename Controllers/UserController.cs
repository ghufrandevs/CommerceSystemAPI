using CommerceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using CommerceSystemAPI.DTOs;
namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController: ControllerBase
    {
        AppDbContext contex = new AppDbContext();

        [HttpPost("Register")]
        public IActionResult Register(UserRegisterDTO dto)
        {
            if (contex.Users.Any(u => u.UserEmail == dto.UserEmail))
            {
                return BadRequest("This Email Already Exists");
            }

            User user = new User()
            {
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                UserPassword = dto.UserPassword,
                UserPhone = dto.UserPhone,
                Role = "Customer",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            contex.Users.Add(user);
            contex.SaveChanges();

            return Ok("Account Created Successfully");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = contex.Users
        .FirstOrDefault(u =>
            u.UserEmail == dto.UserEmail&&
            u.UserPassword == dto.UserPassword);

            if (user == null)
            {
                return BadRequest("Invalid Email Or Password");
            }

            if (!user.IsActive)
            {
                return BadRequest("Your account is inactive");
            }

            return Ok("Login Successful");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(User user) 
        {
            contex.Users.Add(user);
            contex.SaveChanges();

            return Ok("User Added Successfully");

        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers() 
        {
            var usersDto = contex.Users
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

        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id) 
        {
            var user = contex.Users.Find(id);

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

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(int id, User user)
        {
            var usr = contex.Users.Find(id);

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

            contex.Users.Update(usr);
            contex.SaveChanges();

            return Ok("User Updated Successfully");
        }





        }

}
