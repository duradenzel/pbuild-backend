using pbuild_domain.Entities;
using pbuild_domain.Interfaces;
using pbuild_business.Factories;

using pbuild_data.Repositories;

namespace pbuild_business.Services
{
    public class TeamService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        //private readonly ITeamRepository _teamRepository;

        public TeamService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public async Task<Team> SaveTeamAsync(Team team)
        {
            var teamRepository = _repositoryFactory.CreateRepository<ITeamRepository>();
            return await teamRepository.SaveTeamAsync(team);
        }

        public async Task<List<Team>> GetTeamsAsync()
        {
            var teamRepository = _repositoryFactory.CreateRepository<ITeamRepository>();
            return await teamRepository.GetTeamsAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int teamId)
        {
            var teamRepository = _repositoryFactory.CreateRepository<ITeamRepository>();
            Team selectedTeam = await teamRepository.GetTeamByIdAsync(teamId);
            return selectedTeam;
        }
    
    }
}