using System;

namespace VacationPlanner
{


    public class VacationRequestDTO
    {
        public int Id { get; set; }

        public double UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public double VacDays { get; set; }
    }



}

