using Microsoft.EntityFrameworkCore;

namespace Bookly.Models
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set;  }
        public DbSet<User> Users { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Emotion> Emotions { get; set; }
    }
}
