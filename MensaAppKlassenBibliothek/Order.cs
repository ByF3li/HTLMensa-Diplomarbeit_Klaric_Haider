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

        /*
         DTO werden nur die Felder eingetragen die benötigt werden
        Orders-Menus OrderDTO(Email, List<int> MenuIds)
         */
        public int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string UserEmail { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
