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
        
        public int Id { get; set; }
       [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
       
        public int SalesPointId { get; set; }
        
        public int?  BuyerId { get; set; }
        
        public ICollection<SaleData> SalesData { get; set; }
       
        public decimal TotalAmount
        {
            get
            {
                decimal answer = 0;
                foreach (var saleData in SalesData)
                    answer += saleData.ProductIdAmount;
                return answer;
            }
        }
            
         
        
    }
}