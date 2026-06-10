using CommerceSystemAPI.Models;

namespace CommerceSystemAPI.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.UserEmail == email);
        }
        public bool PhoneExists(string phone)
        {
            return _context.Users.Any(u => u.UserPhone == phone);
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public User? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.UserEmail == email);
        }
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public bool EmailExistsForAnotherUser(string email, int userId)
        {
            return _context.Users.Any(u =>
                u.UserEmail == email &&
                u.UserId != userId);
        }

        public bool PhoneExistsForAnotherUser(string phone, int userId)
        {
            return _context.Users.Any(u =>
                u.UserPhone == phone &&
                u.UserId != userId);
        }

    }
}
