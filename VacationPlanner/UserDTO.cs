namespace VacationPlanner
{
    public class UserDTO
    {
        public int Name { get; set; }
        public byte[] Password { get; set; }
    }
    public class VacationDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // Other vacation-related properties
    }

    
}
