using Microsoft.EntityFrameworkCore;

namespace PostcardApp.Models
{
    public class PostcardContext : DbContext
    {
        public PostcardContext(DbContextOptions<PostcardContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<History> Histories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=postcard.db");
        }
    }
}