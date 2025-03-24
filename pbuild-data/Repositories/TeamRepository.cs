using pbuild_domain.Entities;
using pbuild_domain.Interfaces;
using pbuild_data.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace pbuild_data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       public async Task<Team> SaveTeamAsync(Team team)
        {
            Debug.WriteLine(team);
            _context.Teams.Add(team);

            foreach (var pokemon in team.Pokemons)
            {
                pokemon.TeamId = team.Id; 
                _context.Pokemons.Add(pokemon);
            }

            await _context.SaveChangesAsync();
            return team;
        }


        public async Task<List<Team>> GetTeamsAsync()
        {
            return await _context.Teams.Include(t => t.Pokemons).ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int teamId)
        {
            return await _context.Teams
                .Where(t => t.Id == teamId)
                .Include(t => t.Pokemons)
                .FirstOrDefaultAsync();
        }

        public async Task<Team> UpdateTeamAsync(int teamId, Team updatedTeam)
        {
            var existingTeam = await _context.Teams.Include(t => t.Pokemons)
                                                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (existingTeam == null)
            {
                return null;
            }

            existingTeam.Name = updatedTeam.Name;
            
            _context.Pokemons.RemoveRange(existingTeam.Pokemons);
            existingTeam.Pokemons.Clear();
            
            foreach (var pokemon in updatedTeam.Pokemons)
            {
                pokemon.TeamId = teamId;
                _context.Pokemons.Add(pokemon);
                existingTeam.Pokemons.Add(pokemon);
            }

            await _context.SaveChangesAsync();
            return existingTeam;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            var team = await _context.Teams.Include(t => t.Pokemons).FirstOrDefaultAsync(t => t.Id == teamId);
            
            if (team == null)
            {
                return false;
            }

            _context.Pokemons.RemoveRange(team.Pokemons);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}