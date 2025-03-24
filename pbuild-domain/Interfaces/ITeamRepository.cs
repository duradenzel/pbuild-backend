using pbuild_domain.Entities;

namespace pbuild_domain.Interfaces
{
    public interface ITeamRepository
    {
        Task<Team> SaveTeamAsync(Team team);
        Task<List<Team>> GetTeamsAsync();
        Task<Team> GetTeamByIdAsync(int teamId);

        Task<Team> UpdateTeamAsync(int teamId, Team updatedTeam);
        Task<bool> DeleteTeamAsync(int teamId);

    }
}