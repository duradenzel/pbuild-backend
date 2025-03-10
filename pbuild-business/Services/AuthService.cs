using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt;
using pbuild_domain.Interfaces;


using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using pbuild_data.Repositories;
using pbuild_domain.Entities;

public class AuthService
{
   private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public AuthService(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    public string GenerateToken(User user)
    {
        var secretKey = _config["JwtSettings:Secret"];
        var issuer = _config["JwtSettings:Issuer"];
        var audience = _config["JwtSettings:Audience"];
        var expiryMinutes = Convert.ToInt32(_config["JwtSettings:ExpiryMinutes"]);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool VerifyPassword(string inputPassword, string storedPasswordHash)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, storedPasswordHash);
    }

    public User AuthenticateUser(string email, string password)
    {
        var user = _userRepository.GetUserByEmail(email);
        if (user == null || !VerifyPassword(password, user.Password))
        {
            return null;
        }
        return user;
    }

    public void CreateUser(User user)
    {
        _userRepository.AddUser(user);
    }

    
}
