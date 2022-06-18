using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class SalesPoint
    {
       
        
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProvidedProduct> ProvidedProducts { get; set; }
    }
}