using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace IntelWash.Model
{
    public class ProvidedProduct
    {
        [Key]
        public int Id { get; set; }
        
        public Product? Product { get; set; }
        [Required]
        public int ProductId { get; set; }
       
        [Required]
        public int ProductQuantity { get; set; }
        
        public SalesPoint? SalesPoint { get; set; }
        public int SalesPointId { get; set; }
    }
}