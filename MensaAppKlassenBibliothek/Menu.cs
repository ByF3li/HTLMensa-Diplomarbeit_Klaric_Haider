using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class Menu : IComparable<Menu>
    {
        public int MenuId { get; set; }
        public int WhichMenu { get; set; }
        public string Starter { get; set; }
        public string MainCourse { get; set; }
        public decimal Price { get; set; }
        public DateOnly Date { get; set; }
        public List<Order>? Orders { get; set; } = new List<Order>();
        public List<MenuShoppingCartItem> ShoppingCartItems { get; set; } = new List<MenuShoppingCartItem>();


        public int CompareTo(Menu? other)
        {
            if (this.Date.CompareTo(other.Date) == 0)
            {
                return this.WhichMenu.CompareTo(other.WhichMenu);
            }
            return this.Date.CompareTo(other.Date);
        }
    }

}
