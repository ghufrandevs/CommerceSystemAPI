using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Repositories;
using Microsoft.Identity.Client;
using CommerceSystemAPI.Models;

namespace CommerceSystemAPI.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;
        private readonly EmailService _emailService;
        private readonly JwtService _jwtService;

        public UserService(
            UserRepository userRepository,
            PasswordService passwordService,
            EmailService emailService,
            JwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public string Register(UserRegisterDTO dto)
        {
            if (_userRepository.EmailExists(dto.UserEmail))
            {
                return "This Email Already Exists";
            }

            if (_userRepository.PhoneExists(dto.UserPhone))
            {
                return "This Phone Number Already Exists";
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

            _userRepository.AddUser(user);

            _emailService.SendEmail(
                user.UserEmail,
                "Welcome",
                $"Hello {user.UserName}, your account has been created successfully.");

            return "Account Created Successfully";
        }
        public object Login(LoginDTO dto)
        {
            var user = _userRepository.GetByEmail(dto.UserEmail);

            if (user == null)
            {
                return "Invalid Email Or Password";
            }

            bool isValidPassword =
                _passwordService.VerifyPassword(
                    dto.UserPassword,
                    user.UserPassword);

            if (!isValidPassword)
            {
                return "Invalid Email Or Password";
            }

            if (!user.IsActive)
            {
                return "Your account is inactive";
            }

            string token = _jwtService.GenerateToken(user);

            return new
            {
                Message = "Login Successful",
                Token = token,
                UserId = user.UserId,
                Role = user.Role
            };
        }
        public List<UserOutputDTO> GetAllUsers()
        {
            return _userRepository.GetAllUsers()
                .Select(u => new UserOutputDTO
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    UserEmail = u.UserEmail,
                    UserPhone = u.UserPhone
                })
                .ToList();
        }
        public UserOutputDTO? GetUserById(int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                return null;
            }

            return new UserOutputDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserPhone = user.UserPhone
            };
        }

        public string UpdateUser(
   int id,
   UserUpdateDTO dto,
   int loggedInUserId,
   bool isAdmin)
        {
            var usr = _userRepository.GetById(id);

            if (usr == null)
            {
                return "User Not Found";
            }

            if (!isAdmin && loggedInUserId != id)
            {
                return "Forbidden";
            }

            if (_userRepository.EmailExistsForAnotherUser(
                dto.UserEmail, id))
            {
                return "Email already exists";
            }

            if (_userRepository.PhoneExistsForAnotherUser(
                dto.UserPhone, id))
            {
                return "This Phone Number Already Exists";
            }

            usr.UserName = dto.UserName;
            usr.UserEmail = dto.UserEmail;
            usr.UserPassword =
                _passwordService.HashPassword(dto.UserPassword);

            usr.UserPhone = dto.UserPhone;

            if (isAdmin)
            {
                usr.Role = dto.Role;
                usr.IsActive = dto.IsActive;
            }

            _userRepository.UpdateUser(usr);

            return "User Updated Successfully";
        }
    }
}
    

