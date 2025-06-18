using BlogApp.DTOs;
using BlogApp.Interfaces;
using BlogDomain.Entities;
using BlogInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Services;

public class UserService : IUserService
{
    private readonly BlogDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(BlogDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            return new AuthResponse
            {
                IsAuthenticated = false,
                Message = "Username already exists."
            };
        }

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            IsAuthenticated = true,
            Message = "Registration successful."
        };
    }

    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return new AuthResponse
            {
                IsAuthenticated = false,
                Message = "Invalid credentials."
            };
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"], 
            Audience = _configuration["Jwt:Audience"], 
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AuthResponse
        {
            IsAuthenticated = true,
            Token = tokenHandler.WriteToken(token)
        };
    }
}
