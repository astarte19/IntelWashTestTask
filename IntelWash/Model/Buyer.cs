using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class Buyer
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Buyer()
        {
            SalesId = new HashSet<SalesId>();
        }
        public ICollection<SalesId> SalesId { get; set; }
        
    }
}