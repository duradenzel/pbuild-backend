using pbuild_domain.Entities;
using pbuild_data.Repositories;
using pbuild_domain.Interfaces;
using pbuild_business.Factories;

namespace pbuild_business.Services
{
    public class UserService
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public UserService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public User GetUserById(int id)
        {
            var userRepository = _repositoryFactory.CreateRepository<IUserRepository>();
            return userRepository.GetUserById(id);
        }

        public User GetUserByEmail(string email)
        {
            var userRepository = _repositoryFactory.CreateRepository<IUserRepository>();
            return userRepository.GetUserByEmail(email);
        }
    }
}