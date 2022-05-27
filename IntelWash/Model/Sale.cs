using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class Sale
    {
        public Sale()
        {
            SalesData = new HashSet<SaleData>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int SalesPointId { get; set; }
        public SalesPoint? SalePoint { get; set; }
        [Required]
        public int  BuyerId { get; set; }
        public Buyer? Buyer { get; set; }
        
        public ICollection<SaleData> SalesData { get; set; }
        [Required]
        public decimal TotalAmmount { get; set; }
    }
}