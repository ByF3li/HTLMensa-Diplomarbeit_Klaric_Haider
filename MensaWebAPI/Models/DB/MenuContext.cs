using MensaAppKlassenBibliothek;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MensaWebAPI.Models.DB
{
    public class MenuContext : DbContext
    {
        public DbSet<Menu> Menues { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<MenuPerson> MenuPersons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // für den Pomelo-MySQL-Treiber
            string connectionString = "Server=localhost;database=MensaApp;user=DAMensaUser;password=DAMensa23";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure MenuPerson-Menu relationship
            /*modelBuilder.Entity<MenuPerson>()
                .HasOne(mp => mp.Menu)
                .WithMany(m => m.MenuPersons);
                *//*.HasForeignKey(mp => mp.Menu*//*

            // Configure MenuPerson-Person relationship
            modelBuilder.Entity<MenuPerson>()
                .HasOne(mp => mp.Person)
                .WithMany(p => p.MenuPersons)
                *//*.HasForeignKey(mp => mp.Person)*//*;

            // Other configurations...

            //base.OnModelCreating(modelBuilder);*/
        }

    }
}
