using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class SalesPoint
    {
        public SalesPoint()
        {
            ProvidedProducts = new HashSet<ProvidedProduct>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProvidedProduct> ProvidedProducts { get; set; }
    }
}