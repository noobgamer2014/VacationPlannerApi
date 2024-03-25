using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VacationPlanner.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "TeamLeader" or "User" or "HR"
        public string Email { get; set; }
        public int Superior { get; set; }
        public int Appartment { get; set; } // "Entwicklung(1)" or "QS(2)" or "Support(3)"
        public int VacationDays { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }

        


        

    }
}