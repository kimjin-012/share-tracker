using Microsoft.EntityFrameworkCore;

namespace StarTracker.Models
{
    public class StarTrackerContext : DbContext
    {
        public StarTrackerContext(DbContextOptions options) : base(options) { }
 
        // for every model / entity that is going to be part of the db
        // the names of these properties will be the names of the tables in the db
        public DbSet<User> Users { get; set; }
        public DbSet<Star> Stars { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}