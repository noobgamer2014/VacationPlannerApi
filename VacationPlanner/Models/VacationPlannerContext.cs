using Microsoft.EntityFrameworkCore;

namespace VacationPlanner.Models
{
    public class VacationPlannerContext : DbContext
    {
        public VacationPlannerContext(DbContextOptions<VacationPlannerContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Vacation> Vacations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=HASELHORST-LAP\\SQL2019;Database=Vacationplanner;User ID=sa;Password=sa;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
);
        }
    }

}
