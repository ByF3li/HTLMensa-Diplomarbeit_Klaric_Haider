using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class Menu : IComparable<Menu>
    {
        public int MenuId { get; set; }
        
        public string Starter { get; set; }
        public string MainCourse { get; set; }
        
        public DateOnly Date { get; set; }

        public List<MenuPerson> MenuPersons { get; set; } = new List<MenuPerson>() { };
	    public PriceForMenu Prices { get; set; }
        
        //Used to sort the List of Menus by date and price
        public int CompareTo(Menu? other)
        {
            if (this.Date.CompareTo(other.Date) == 0)
            {
                return this.Prices.PriceId.CompareTo(other.Prices.PriceId);
            }
            return this.Date.CompareTo(other.Date);
        }
    }
}
