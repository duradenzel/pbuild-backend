using pbuild_domain.Entities;

namespace pbuild_data.Repositories
{
    public interface ITeamRepository
    {
        Task<Team> SaveTeamAsync(Team team);
        Task<List<Team>> GetTeamsAsync();
        Task<Team> GetTeamByIdAsync(int teamId);
    }
}