using MensaAppKlassenBibliothek;
using Microsoft.EntityFrameworkCore;

namespace MensaWebAPI.Models.DB
{
    public class MenuContext : DbContext
    {
        public DbSet<Menu> Menues { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // für den Pomelo-MySQL-Treiber
            string connectionString = "Server=localhost;database=MensaApp;user=DAMensaUser;password=DAMensa23";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
