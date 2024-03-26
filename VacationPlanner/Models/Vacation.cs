using System;
using System.ComponentModel.DataAnnotations;

namespace VacationPlanner.Models
{
    public class Vacation
    {
        
        public int Id { get; set; }
        public double UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; } // "Pending(1)", "Approved(2)", "Rejected(3)"
        public double VacDays {  get; set; } // Days that get Used

        public int Superior { get; set; }
        public System.DateTime RequestDate { get; set; }
        public virtual User User { get; set; }
        
    }
}
