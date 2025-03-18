using Xunit;
using Moq;
using pbuild_business.Services;
using pbuild_domain.Interfaces;
using pbuild_domain.Entities;
using pbuild_business.Factories;

namespace pbuild_tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepositoryFactory> _mockRepositoryFactory;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockRepositoryFactory
                .Setup(f => f.CreateRepository<IUserRepository>())
                .Returns(_mockUserRepository.Object);

            _userService = new UserService(_mockRepositoryFactory.Object);
        }

        [Fact]
        public void GetUserById_ShouldReturnUser_WhenUserExists()
        {
            var userId = 1;
            var email = "huijbers15@outlook.com";

            var expectedUser = new User { Id = 1, Name = "Existing User", Password = "admin", Email = email, Role = "Admin" };

            _mockUserRepository
                .Setup(repo => repo.GetUserById(userId))
                .Returns(expectedUser);

            var result = _userService.GetUserById(userId);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);
        }

        [Fact]
        public void GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var userId = 2;

            _mockUserRepository
                .Setup(repo => repo.GetUserById(userId))
                .Returns((User)null); 

            var result = _userService.GetUserById(userId);

            Assert.Null(result);
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnUser_WhenEmailExists()
        {
            var email = "huijbers15@outlook.com";
            var expectedUser = new User { Id = 1, Name = "Existing User", Password = "admin", Email = email, Role = "Admin" };

            _mockUserRepository
                .Setup(repo => repo.GetUserByEmail(email))
                .Returns(expectedUser);

            var result = _userService.GetUserByEmail(email);

            Assert.NotNull(result);
            Assert.Equal(expectedUser.Email, result.Email);
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            var email = "nonexistent@example.com";

            _mockUserRepository
                .Setup(repo => repo.GetUserByEmail(email))
                .Returns((User)null);

            var result = _userService.GetUserByEmail(email);

            Assert.Null(result);
        }
    }
}
