using Microsoft.EntityFrameworkCore;
using Xunit;
using pbuild_domain.Entities;
using pbuild_data.Database;
using pbuild_business.Services;
using pbuild_data.Repositories;
using pbuild_domain.Interfaces;
using pbuild_business.Factories;
using Moq;
using Microsoft.EntityFrameworkCore.InMemory;

public class TeamIntegrationTests
{
    private TeamService CreateTeamService(out ApplicationDbContext context)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new ApplicationDbContext(options);

        var teamRepository = new TeamRepository(context);

        var factoryMock = new Mock<IRepositoryFactory>();
        factoryMock.Setup(f => f.CreateRepository<ITeamRepository>()).Returns(teamRepository);

        return new TeamService(factoryMock.Object);
    }


    [Fact]
    public async Task SaveTeamAsync_SavesTeamAndPokemons()
    {
        var service = CreateTeamService(out var context);
        var team = new Team
        {
            Name = "cool team",
            Pokemons = new List<Pokemon>
            {
                new Pokemon { Name = "Empoleon" },
                new Pokemon { Name = "Infernape" }
            }
        };

        var result = await service.SaveTeamAsync(team);

        var teamInDb = context.Teams.Include(t => t.Pokemons).FirstOrDefault();
        Assert.NotNull(teamInDb);
        Assert.Equal("Test Team", teamInDb.Name);
        Assert.Equal(2, teamInDb.Pokemons.Count);
    }

    [Fact]
    public async Task GetTeamByIdAsync_ReturnsCorrectTeam()
    {
        var service = CreateTeamService(out var context);

        var team = new Team
        {
            Name = "get test",
            Pokemons = new List<Pokemon>()
        };
        context.Teams.Add(team);
        context.SaveChanges();

        var result = await service.GetTeamByIdAsync(team.Id);

        Assert.NotNull(result);
        Assert.Equal("get test", result.Name);
    }
    
    [Fact]
    public async Task DeleteTeamAsync_RemovesTeamAndPokemons()
    {
        var service = CreateTeamService(out var context);
        var team = new Team
        {
            Name = "delete test",
            Pokemons = new List<Pokemon> { new Pokemon { Name = "Pikachu" } }
        };
        context.Teams.Add(team);
        context.SaveChanges();

        var result = await service.DeleteTeamAsync(team.Id);

        Assert.True(result);
        Assert.Empty(context.Teams.ToList());
        Assert.Empty(context.Pokemons.ToList());
    }



}