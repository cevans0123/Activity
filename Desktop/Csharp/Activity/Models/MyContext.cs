using Microsoft.EntityFrameworkCore;

namespace Activity.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Actv> Activities { get; set; }
        public DbSet<Participant> Participants { get; set; }
    }
}