using Microsoft.EntityFrameworkCore;
using SensorAPI.Models;

namespace SensorAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
    }

    public DbSet<Sensor> Sensores { get; set; }
}
}
