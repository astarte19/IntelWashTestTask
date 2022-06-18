using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IntelWash.Model
{
    public class SaleData
    {
        private readonly ApplicationContext _context;
        
        public SaleData(ApplicationContext context)
        {
            _context = context;
        }
        
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        
        public int ProductQuantity { get; set; }
        
        public decimal ProductIdAmount
        {
            get
            {
                if (_context is null)
                    return 0;
                var product = _context.Products.FirstOrDefault(p => p.Id == ProductId);
                return ProductQuantity * product.Price;
            }
        }
    }
}