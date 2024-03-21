using MensaAppKlassenBibliothek;
using Microsoft.EntityFrameworkCore;

namespace MensaWebsite.Models.DB
{
    public class MenuContext : DbContext
    {
        public DbSet<Menu> Menues { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<MenuPerson> MenuPersons { get; set; }
        public DbSet<PriceForMenu> Prices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // für den Pomelo-MySQL-Treiber
            string connectionString = "Server=localhost;database=databaseName;user=root;password=root_password";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        
        }
    }
}
