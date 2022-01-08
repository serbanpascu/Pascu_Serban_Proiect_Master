using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pascu_Serban_Proiect.Models
{
    public class Brand
    {
        public int ID { get; set; }
        
        [Required]
        [Display(Name = "Brand Name")]
        [StringLength(50)]
        public string BrandName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<BrandedToy> BrandedToys { get; set; }
    }
}
