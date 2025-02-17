using pbuild_domain.Entities;

namespace pbuild_data.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(int id);

        User GetUserByEmail(string email);

        void AddUser(User user);
    }
}