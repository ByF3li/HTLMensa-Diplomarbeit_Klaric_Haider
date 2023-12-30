using MensaAppKlassenBibliothek;

namespace MensaWebsite.Models
{
    public class MenuDTO
    {
        public int MenuId { get; set; }
        public int WhichMenu { get; set; }
        public string Starter { get; set; }
        public string MainCourse { get; set; }
        public DateOnly Date { get; set; }
        public decimal PriceStudent { get; set; }
        public decimal PriceTeacher { get; set; }

    }
}
