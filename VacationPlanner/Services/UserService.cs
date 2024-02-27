using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationPlanner.Models;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task<User> Register(string username, string email, string password);
    // ... other methods
}

public class UserService : IUserService
{
    private readonly VacationPlannerContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(VacationPlannerContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> Authenticate(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Name == username);

        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user; // Authentication successful
            }
        }

        return null; // Authentication failed
    }


    public IPasswordHasher<User> Get_passwordHasher()
    {
        return _passwordHasher;
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
            Role = "User" // Default role; adjust as necessary.
        };

        // Hashing the password
        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }


}
