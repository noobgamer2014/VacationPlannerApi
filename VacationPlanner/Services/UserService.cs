using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VacationPlanner.Models;
using VacationPlanner.Services;
public interface IUserService
{
    Task<(User User, string Token)> Authenticate(string username, string password);
    Task<User> Register(string username, string email, string password);
}

public class UserService : IUserService
{
    private readonly VacationPlannerContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    // Assume TokenService is a service you have that handles token generation
    private readonly TokenService _tokenService;

    public UserService(VacationPlannerContext context, IPasswordHasher<User> passwordHasher, TokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<(User User, string Token)> Authenticate(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Name == username);
        if (user == null) return (null, null);

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success) return (null, null);

        // Generate token with user ID claim
        var claims = new List<Claim>
{
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Ensure user.Id is an integer
        new Claim(ClaimTypes.Name, user.Name),
        // other claims
    };
        string token = _tokenService.GenerateToken(claims);

        return (user, token);
    }

    public async Task<User> Register(string username, string email, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Name == username))
        {
            throw new Exception("Username is already taken");
        }

        var user = new User
        {
            Name = username,
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(null, password),
            Role = "User",
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
