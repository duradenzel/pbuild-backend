using pbuild_domain.Entities;
using pbuild_data.Database;

namespace pbuild_data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                Console.WriteLine($"User with ID {id} not found in the database.");
            }
            else
            {
                Console.WriteLine($"User found: {user.Name}");
            }

            return user;
        }
    
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}