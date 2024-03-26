using System;

namespace VacationPlanner
{
   

    public class VacationRequestDTO
    {
        public DateTime StartDate { get; internal set; }
        public DateTime EndDate { get; internal set; }

        public class VacationRequestDto
        {
            public double UserId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Superior { get; set; }
        }

        // Add other properties as needed
    }


}

