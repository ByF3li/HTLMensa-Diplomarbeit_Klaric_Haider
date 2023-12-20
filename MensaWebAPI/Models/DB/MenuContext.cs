using MensaAppKlassenBibliothek;
using Microsoft.EntityFrameworkCore;

namespace MensaWebAPI.Models.DB
{
    public class MenuContext : DbContext
    {
        public DbSet<Menu> Menues { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<MenuShoppingCartItem> MenuShoppingCartItems { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // für den Pomelo-MySQL-Treiber
            string connectionString = "Server=localhost;database=MensaApp;user=DAMensaUser;password=DAMensa23";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuShoppingCartItem>()
            .HasKey(msc => new { msc.MenuId, msc.ShoppingCartId });

            modelBuilder.Entity<ShoppingCart>().HasMany(sc => sc.MenuItems)
                .WithOne(msc => msc.ShoppingCart)
                .HasForeignKey(msc => msc.ShoppingCartId);

            modelBuilder.Entity<Menu>().HasMany(m => m.ShoppingCartItems)
                .WithOne(msc => msc.Menu)
                .HasForeignKey(msc => msc.MenuId);
        }
    }
}
