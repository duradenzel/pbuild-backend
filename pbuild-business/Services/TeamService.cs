using pbuild_domain.Entities;
using pbuild_data.Repositories;


namespace pbuild_business.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<Team> SaveTeamAsync(Team team)
        {
            return await _teamRepository.SaveTeamAsync(team);
        }

        public async Task<List<Team>> GetTeamsAsync()
        {
            return await _teamRepository.GetTeamsAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int teamId)
        {
            Team selectedTeam = await _teamRepository.GetTeamByIdAsync(teamId);
            return selectedTeam;
        }
    
    }
}