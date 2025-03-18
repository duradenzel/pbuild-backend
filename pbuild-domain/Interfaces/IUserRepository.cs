using pbuild_domain.Entities;

namespace pbuild_domain.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int id);

        User GetUserByEmail(string email);

        void AddUser(User user);
    }
}