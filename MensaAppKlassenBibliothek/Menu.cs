using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class Menu
    {
        public int MenuId { get; set; }
        public int WhichMenu { get; set; }
        public string Starter { get; set; }
        public string MainCourse { get; set; }
        public decimal Price { get; set; }
        public DateOnly Date { get; set; }
        public List<Order> Orders { get; set; }

    }
}
