using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VacationPlanner.Services;
using VacationPlanner.Models;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Realms.Sync;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using static VacationPlanner.VacationRequestDTO;

namespace VacationPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VacationController : ControllerBase
    {
        private readonly IVacationService _vacationService;

        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }

        // Retrieve the current user ID from the User claims
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID claim not found or is empty.");
            }

            return int.Parse(userIdClaim);
        }

        [HttpPost("requestVacation")]
        public async Task<IActionResult> RequestVacation([FromBody] VacationRequestDto vacationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var vacDays = CalculateVacationDays(vacationRequest.StartDate, vacationRequest.EndDate);
                var vacation = new Vacation
                {
                    UserId = vacationRequest.UserId,
                    StartDate = vacationRequest.StartDate,
                    EndDate = vacationRequest.EndDate,
                    VacDays = vacDays,
                    StatusId = 1, // Pending by default
                    RequestDate = DateTime.UtcNow // Use UTC for consistency
                };

                await _vacationService.AddVacationRequest(vacation);

                return Ok(new { message = "Vacation request submitted successfully." });
            }
            catch (Exception ex)
            {
                // Log exception details here
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        private int CalculateVacationDays(DateTime start, DateTime end)
        {
            int diffDays = 0;
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                // Exclude weekends
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    diffDays++;
                }
            }
            return diffDays;
        }


        [HttpGet("leftoverDays")]
        [Authorize]
        public ActionResult<int> GetLeftoverDays()
        {
            var userId = GetCurrentUserId(); 
            var daysLeft = _vacationService.GetLeftoverDays(userId);
            return Ok(daysLeft);
        }

        // GET: api/vacation/recentVacations/{userId}
        [HttpGet("recentVacations")]
        [Authorize]
        public ActionResult<IEnumerable<VacationRequestDTO>> GetRecentVacations()
        {
            try
            {
                var userId = GetCurrentUserId();
                var recentVacations = _vacationService.GetRecentVacations(userId);
                return Ok(recentVacations);
            }
            catch (System.Exception ex)
            {
                // Handle the exception appropriately
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        
    }
}
