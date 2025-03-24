using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pbuild_business.Services;
using pbuild_domain;
using pbuild_domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PBuild.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamService _teamService;

        public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveTeam([FromBody] Team team)
            {
                var token = Request.Headers["Authorization"].ToString();
                Console.WriteLine($"Received Token: {token}");

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                if (team == null || string.IsNullOrWhiteSpace(team.Name) || team.Pokemons == null)
                    return BadRequest("Invalid team data");

                
                foreach (var pokemon in team.Pokemons)
                {
                    pokemon.Team = team; 
                }

                var savedTeam = await _teamService.SaveTeamAsync(team);
                return Ok(savedTeam);
            }

        [HttpGet("list")]
        public async Task<IActionResult> GetTeams()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var teams = await _teamService.GetTeamsAsync();
            return Ok(teams);
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeamByIdAsync(int teamId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var team = await _teamService.GetTeamByIdAsync(teamId);
            if (team == null)
            {
                return NotFound($"Team with ID {teamId} not found.");
            }

            return Ok(team);  
        }

        [HttpPut("update/{teamId}")]
        public async Task<IActionResult> UpdateTeam(int teamId, [FromBody] Team updatedTeam)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (updatedTeam == null || string.IsNullOrWhiteSpace(updatedTeam.Name) || updatedTeam.Pokemons == null)
                return BadRequest("Invalid team data");

            var existingTeam = await _teamService.GetTeamByIdAsync(teamId);
            if (existingTeam == null)
            {
                return NotFound($"Team with ID {teamId} not found.");
            }

            var updatedResult = await _teamService.UpdateTeamAsync(teamId, updatedTeam);
            return Ok(updatedResult);
        }

        [HttpDelete("delete/{teamId}")]
        public async Task<IActionResult> DeleteTeam(int teamId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var deleted = await _teamService.DeleteTeamAsync(teamId);
            if (!deleted)
            {
                return NotFound($"Team with ID {teamId} not found.");
            }

            return NoContent();
        }


        


    }
}
