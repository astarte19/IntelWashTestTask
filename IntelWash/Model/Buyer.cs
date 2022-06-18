using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class Buyer
    {
        
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public  List<SalesId> SalesId { get; set; }
        
    }

    public class SalesId
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
    }
}