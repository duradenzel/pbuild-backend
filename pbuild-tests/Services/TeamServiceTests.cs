using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

using pbuild_domain.Entities;
using pbuild_domain.Interfaces;
using pbuild_business.Services;
using pbuild_business.Factories;

public class TeamServiceTests
{
    private readonly Mock<IRepositoryFactory> _mockFactory;
    private readonly Mock<ITeamRepository> _mockTeamRepo;
    private readonly TeamService _teamService;

    public TeamServiceTests()
    {
        _mockFactory = new Mock<IRepositoryFactory>();
        _mockTeamRepo = new Mock<ITeamRepository>();

        _mockFactory.Setup(f => f.CreateRepository<ITeamRepository>())
                    .Returns(_mockTeamRepo.Object);

        _teamService = new TeamService(_mockFactory.Object);
    }

    [Fact]
    public async Task SaveTeamAsync_ShouldReturnTeam_WhenSaveIsSuccessful()
    {
        var team = new Team { Id = 1, Name = "test team" };
        _mockTeamRepo.Setup(repo => repo.SaveTeamAsync(team)).ReturnsAsync(team);

        var result = await _teamService.SaveTeamAsync(team);

        Assert.Equal(team, result);
        _mockTeamRepo.Verify(r => r.SaveTeamAsync(team), Times.Once);
    }

    [Fact]
    public async Task GetTeamsAsync_ShouldReturnListOfTeams_WhenTeamsExist()
    {
        var teams = new List<Team> { new Team { Id = 1 }, new Team { Id = 2 } };
        _mockTeamRepo.Setup(repo => repo.GetTeamsAsync()).ReturnsAsync(teams);

        var result = await _teamService.GetTeamsAsync();

        Assert.Equal(teams, result);
        _mockTeamRepo.Verify(r => r.GetTeamsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTeamByIdAsync_ShouldReturnTeam_WhenTeamExists()
    {
        var team = new Team { Id = 1, Name = "test team" };
        _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(1)).ReturnsAsync(team);

        var result = await _teamService.GetTeamByIdAsync(1);

        Assert.Equal(team, result);
        _mockTeamRepo.Verify(r => r.GetTeamByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetTeamByIdAsync_ShouldReturnNull_WhenTeamDoesNotExist()
    {
        _mockTeamRepo.Setup(repo => repo.GetTeamByIdAsync(99)).ReturnsAsync((Team?)null);

        var result = await _teamService.GetTeamByIdAsync(99);

        Assert.Null(result);
        _mockTeamRepo.Verify(r => r.GetTeamByIdAsync(99), Times.Once);
    }


    //TODO: create new method or update current method to also upate the included pokemon 
    [Fact]
    public async Task UpdateTeamAsync_ShouldReturnUpdatedTeam_WhenUpdateIsSuccessful()
    {
        var updatedTeam = new Team { Id = 1, Name = "test team" };
        _mockTeamRepo.Setup(repo => repo.UpdateTeamAsync(1, updatedTeam)).ReturnsAsync(updatedTeam);

        var result = await _teamService.UpdateTeamAsync(1, updatedTeam);

        Assert.Equal(updatedTeam, result);
        _mockTeamRepo.Verify(r => r.UpdateTeamAsync(1, updatedTeam), Times.Once);
    }

    [Fact]
    public async Task DeleteTeamAsync_ShouldReturnTrue_WhenTeamIsDeleted()
    {
        _mockTeamRepo.Setup(repo => repo.DeleteTeamAsync(1)).ReturnsAsync(true);

        var result = await _teamService.DeleteTeamAsync(1);

        Assert.True(result);
        _mockTeamRepo.Verify(r => r.DeleteTeamAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteTeamAsync_ShouldReturnFalse_WhenTeamDoesNotExist()
    {
        _mockTeamRepo.Setup(repo => repo.DeleteTeamAsync(99)).ReturnsAsync(false);

        var result = await _teamService.DeleteTeamAsync(99);

        Assert.False(result);
        _mockTeamRepo.Verify(r => r.DeleteTeamAsync(99), Times.Once);
    }

    [Fact]
    public async Task SaveTeamAsync_ShouldThrowException_WhenRepositoryIsUnavailable()
    {
        var team = new Team { Id = 1, Name = "shouldnt work" };
        var repositoryFactory = new Mock<IRepositoryFactory>();
        repositoryFactory.Setup(f => f.CreateRepository<ITeamRepository>())
                     .Throws(new InvalidOperationException("oh no oopsie"));

        var teamService = new TeamService(repositoryFactory.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => teamService.SaveTeamAsync(team));
    }
}
