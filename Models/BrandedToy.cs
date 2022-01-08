using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pascu_Serban_Proiect.Models
{
    public class BrandedToy
    {
        public int BrandID { get; set; }
        public int ToyID { get; set; }
        public Brand Brand { get; set; }
        public Toy Toy { get; set; }
    }
}
