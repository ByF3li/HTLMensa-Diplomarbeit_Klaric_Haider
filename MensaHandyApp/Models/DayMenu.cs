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
        //Used to display the Weekly Menus in a formated way => 3 menus per date/day 
        public DateOnly Date { get; set; }
        public List<Menu> Menus { get; set; } = new List<Menu> { };
    }
}
