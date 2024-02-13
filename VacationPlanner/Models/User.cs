using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VacationPlanner.Models
{
    public class User
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }
        public string Role { get; set; } // "TeamLeader" or "User" or "HR"
        public string Email { get; set; }
        public int Superior { get; set; }
        public int Appartment {  get; set; } // "Entwicklung(1)" or "QS(2)" or "Support(3)"
        public double MaxVacationDays {  get; set; } 
        public virtual ICollection<Vacation> Vacations { get; set; }
    }
}