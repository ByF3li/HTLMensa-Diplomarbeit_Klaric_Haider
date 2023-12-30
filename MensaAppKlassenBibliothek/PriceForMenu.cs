using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaAppKlassenBibliothek
{
    public class PriceForMenu
    {
        [Key]
        public int PriceId { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal PriceStudent { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal PriceTeacher { get; set; }
    }
}
