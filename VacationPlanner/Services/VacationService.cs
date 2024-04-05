namespace VacationPlanner.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Microsoft.EntityFrameworkCore;
    using VacationPlanner.Models;

    public interface IVacationService
    {
        double GetLeftoverDays(double userId);
        Task AddVacationRequest(Vacation vacation);
        IEnumerable<VacationRequestDTO> GetRecentVacations(int userId);
        IEnumerable<VacationRequestDTO> GetAllVacationRequests();
        Task<IEnumerable<VacationRequestDTO>> GetVacationsAsync(int userId,int? statusId, DateTime? startDate, DateTime? endDate);
    }

    public class VacationService : IVacationService
    {
        private readonly VacationPlannerContext _context;
        private readonly HttpClient _httpClient;

        public VacationService(VacationPlannerContext dbContext)
        {
            _context = dbContext;
            
        }

        public async Task AddVacationRequest(Vacation vacation)
        {
            _context.Vacations.Add(vacation);
            _context.SaveChanges();
        }

        public IEnumerable<VacationRequestDTO> GetAllVacationRequests()
        {
            return (IEnumerable<VacationRequestDTO>)_context.Vacations.ToList();
        }

        public async Task<IEnumerable<VacationRequestDTO>> GetVacationsAsync(int userId, int? statusId, DateTime? startDate, DateTime? endDate)
        {
            
            
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new ArgumentException("User not found", nameof(userId));
            }


            var query = _context.Vacations.Where(v => v.UserId == userId);

            // Apply additional filters if provided.
            if (statusId.HasValue)
            {
                query = query.Where(v => v.StatusId == statusId.Value);
            }
            if (startDate.HasValue)
            {
                query = query.Where(v => v.StartDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(v => v.EndDate <= endDate.Value);
            }

            // Project the result to a DTO to avoid sending entity models directly.
            var vacationRequests = await query.Select(v => new VacationRequestDTO
            {
                Id = v.Id,
                UserId = v.UserId,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                StatusId = v.StatusId,
                VacDays = v.VacDays
            }).ToListAsync();

            return vacationRequests;
        }

        public double GetLeftoverDays(double userId)
        {
            var currentYear = DateTime.Now.Year;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var totalVacationDays = user.VacationDays; // Assuming this is a property of User
            var usedDays = _context.Vacations
                                   .Where(v => v.UserId == userId && v.StartDate.Year == currentYear)
                                   .Sum(v => v.VacDays);

            return totalVacationDays - usedDays;
        }

        public IEnumerable<VacationRequestDTO> GetRecentVacations(int userId)
        {
            // Fetching the 5 most recent vacation requests
            return _context.Vacations
                           .Where(v => v.UserId == userId)
                           .OrderByDescending(v => v.RequestDate)
                           .Take(5)
                           .Select(v => new VacationRequestDTO
                           {
                               StartDate = v.StartDate,
                               EndDate = v.EndDate,
                               // Map other necessary properties
                           })
                           .ToList();
        }
        
    }
}
