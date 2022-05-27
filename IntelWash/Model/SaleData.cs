using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class SaleData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        [Required]
        public decimal ProductIdAmount { get; set; }
        
        public Sale? Sale { get; set; }
        
        public int SaleId { get; set; }
    }
}