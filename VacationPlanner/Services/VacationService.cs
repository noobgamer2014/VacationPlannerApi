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
        void AddVacationRequest(Vacation vacationRequest);
        IEnumerable<VacationRequestDTO> GetRecentVacations(int userId);
        IEnumerable<VacationRequestDTO> GetAllVacationRequests();
    }

    public class VacationService : IVacationService
    {
        private readonly VacationPlannerContext _context;
        private readonly HttpClient _httpClient;

        public VacationService(VacationPlannerContext dbContext)
        {
            _context = dbContext;
            
        }

        public void AddVacationRequest(Vacation vacationRequest)
        {
            _context.Vacations.Add(vacationRequest);
            _context.SaveChanges();
        }

        public IEnumerable<VacationRequestDTO> GetAllVacationRequests()
        {
            return (IEnumerable<VacationRequestDTO>)_context.Vacations.ToList();
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
