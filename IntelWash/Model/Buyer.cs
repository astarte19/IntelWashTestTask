using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntelWash.Model
{
    public class Buyer
    {
        [Key]
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public  ICollection<SalesId> SalesId { get; set; }
        
    }
}