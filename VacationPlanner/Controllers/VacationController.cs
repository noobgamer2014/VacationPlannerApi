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
