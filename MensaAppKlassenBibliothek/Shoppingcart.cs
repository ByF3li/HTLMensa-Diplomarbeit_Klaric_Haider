using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class Shoppingcart
    {
        //would be used for PayPal, to send this data to the server 
        public string Total { get; set; }
        public string ProductIdentifiers { get; set; }
    }
}
