using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class SalesId
    {
        [Key]
        public int Id { get; set; }
        
        public int SaleId { get; set; }
        
        public int BuyerId { get; set; }
        public Buyer? Buyer { get; set; }
    }
}