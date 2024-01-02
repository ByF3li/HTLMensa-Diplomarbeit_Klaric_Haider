using MensaAppKlassenBibliothek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaHandyApp.Models
{
    public class DayMenu
    {
        public DateOnly Date { get; set; }
        public List<Menu> Menus { get; set; } = new List<Menu> { };
    }
}
