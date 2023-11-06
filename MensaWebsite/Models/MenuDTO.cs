namespace MensaWebsite.Models
{
    public class MenuDTO
    {
        public int MenuId { get; set; }
        public int WhichMenu { get; set; }
        public string Starter { get; set; }
        public string MainCourse { get; set; }
        public decimal Price { get; set; }
        public DateOnly Date { get; set; }
    }
}
