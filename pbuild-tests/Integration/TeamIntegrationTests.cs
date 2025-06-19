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
        Assert.Equal("cool team", teamInDb.Name);
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
    
   [Fact]
    public async Task SaveTeamAsync_WithNullTeam_ThrowsNullReferenceException()
    {
        var service = CreateTeamService(out var context);

        await Assert.ThrowsAsync<NullReferenceException>(
            async () => await service.SaveTeamAsync(null));
    }

    [Fact]
    public async Task SaveTeamAsync_WithEmptyTeamName_SavesTeamWithEmptyName()
    {
        var service = CreateTeamService(out var context);
        var team = new Team
        {
            Name = "",
            Pokemons = new List<Pokemon>()
        };

        var result = await service.SaveTeamAsync(team);

        Assert.NotNull(result);
        Assert.Equal("", result.Name);

        var teamInDb = context.Teams.FirstOrDefault();
        Assert.NotNull(teamInDb);
        Assert.Equal("", teamInDb.Name);
    }

     [Fact]
    public async Task SaveTeamAsync_WithNullPokemonsList_ThrowsNullReferenceException()
    {
        var service = CreateTeamService(out var context);
        var team = new Team
        {
            Name = "test team",
            Pokemons = null
        };

        await Assert.ThrowsAsync<NullReferenceException>(
            async () => await service.SaveTeamAsync(team));
    }

    [Fact]
    public async Task SaveTeamAsync_WithEmptyPokemonsList_SavesTeamWithoutPokemons()
    {
        var service = CreateTeamService(out var context);
        var team = new Team
        {
            Name = "empty team",
            Pokemons = new List<Pokemon>()
        };

        var result = await service.SaveTeamAsync(team);

        Assert.NotNull(result);
        Assert.Equal("empty team", result.Name);
        Assert.Empty(result.Pokemons);

        var teamInDb = context.Teams.Include(t => t.Pokemons).FirstOrDefault();
        Assert.NotNull(teamInDb);
        Assert.Empty(teamInDb.Pokemons);
    }

    [Fact]
    public async Task GetTeamByIdAsync_WithNonExistentId_ReturnsNull()
    {
        var service = CreateTeamService(out var context);

        var result = await service.GetTeamByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetTeamByIdAsync_WithNegativeId_ReturnsNull()
    {
        var service = CreateTeamService(out var context);

        var result = await service.GetTeamByIdAsync(-1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetTeamByIdAsync_WithZeroId_ReturnsNull()
    {
        var service = CreateTeamService(out var context);

        var result = await service.GetTeamByIdAsync(0);

        Assert.Null(result);
    }

   
    [Fact]
    public async Task DeleteTeamAsync_WithNonExistentId_ReturnsFalse()
    {
        var service = CreateTeamService(out var context);

        var result = await service.DeleteTeamAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteTeamAsync_WithNegativeId_ReturnsFalse()
    {
        var service = CreateTeamService(out var context);

        var result = await service.DeleteTeamAsync(-1);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteTeamAsync_WithZeroId_ReturnsFalse()
    {
        var service = CreateTeamService(out var context);

        var result = await service.DeleteTeamAsync(0);

        Assert.False(result);
    }

    [Fact]
    public async Task GetTeamsAsync_WithEmptyDatabase_ReturnsEmptyList()
    {
        var service = CreateTeamService(out var context);

        var result = await service.GetTeamsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTeamsAsync_WithMultipleTeams_ReturnsAllTeams()
    {
        var service = CreateTeamService(out var context);
        
        var team1 = new Team { Name = "Team 1", Pokemons = new List<Pokemon>() };
        var team2 = new Team { Name = "Team 2", Pokemons = new List<Pokemon> { new Pokemon { Name = "Pikachu" } } };
        var team3 = new Team { Name = "Team 3", Pokemons = new List<Pokemon>() };
        
        context.Teams.AddRange(team1, team2, team3);
        context.SaveChanges();

        var result = await service.GetTeamsAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, t => t.Name == "Team 1");
        Assert.Contains(result, t => t.Name == "Team 2");
        Assert.Contains(result, t => t.Name == "Team 3");
    }

    [Fact]
    public async Task DeleteTeamAsync_AfterTeamAlreadyDeleted_ReturnsFalse()
    {
        var service = CreateTeamService(out var context);
        var team = new Team
        {
            Name = "test team",
            Pokemons = new List<Pokemon> { new Pokemon { Name = "Pikachu" } }
        };
        context.Teams.Add(team);
        context.SaveChanges();
        var teamId = team.Id;

        var firstResult = await service.DeleteTeamAsync(teamId);
        Assert.True(firstResult);

        var secondResult = await service.DeleteTeamAsync(teamId);
        Assert.False(secondResult);
    }

    [Fact]
    public async Task UpdateTeamAsync_AfterTeamDeleted_ReturnsNull()
    {
        var service = CreateTeamService(out var context);
        var team = new Team
        {
            Name = "original",
            Pokemons = new List<Pokemon>()
        };
        context.Teams.Add(team);
        context.SaveChanges();
        var teamId = team.Id;

        await service.DeleteTeamAsync(teamId);

        var updatedTeam = new Team
        {
            Name = "test",
            Pokemons = new List<Pokemon>()
        };
        var result = await service.UpdateTeamAsync(teamId, updatedTeam);

        Assert.Null(result);
    }


}