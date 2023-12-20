using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public  class ShoppingCart
    {
        public int ShoppingCartId { get; set; }

        public List<MenuShoppingCartItem> MenuItems { get; set; } = new List<MenuShoppingCartItem>();
    }
}
