using incidents_observer.Models;
using Microsoft.EntityFrameworkCore;

namespace incidents_observer.Context
{
    public class AppDbContext:DbContext
    {
        public DbSet<Message> EventMessage { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
