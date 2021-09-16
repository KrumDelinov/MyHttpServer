using Microsoft.EntityFrameworkCore;

namespace DemoApp
{
    public class AplicatinDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.UseSqlServer("Server=DESKTOP-TCS836O\\SQLEXPRESS01;Database=DemoApp;Integrated Security=True;");
        }
        public DbSet<Tweet> Tweets { get; set; }
    }
}
