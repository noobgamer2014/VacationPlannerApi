using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VacationPlanner.Models;


    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        
    }

    public class UserService : IUserService
    {
        private readonly VacationPlannerContext _context;

        public UserService(VacationPlannerContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string username, string password)
        {

            // Retrieve the user by username
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Name == username);


            if (user == null || !VerifyPasswordHash(password, user.Password))
            {
                // Authentication failed
                return null;
            }

            // Authentication successful
            return user;
        }

        private bool VerifyPasswordHash(string providedPassword, byte[] storedHash)
        {
            // Password code here
            throw new NotImplementedException();
        }
    }

