﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class MenuPerson
    {
        public int MenuPersonId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string PaymentStatus { get; set; }
        public bool Activated { get; set; }
        public bool InShoppingcart { get; set; }

        public string PaypalOrderId { get; set; }

        //Navigation Properties
        public required Person Person { get; set; }
        public required Menu Menu { get; set; }
    }
}
