using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string UserEmail { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
