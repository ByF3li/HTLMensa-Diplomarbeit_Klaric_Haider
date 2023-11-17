using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class DtoOrder
    {
        public DateOnly OrderDate { get; set; }
        public string UserEmail { get; set; }
        public List<int> MenuIds { get; set; } = new List<int>();
    }
}
