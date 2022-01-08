using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pascu_Serban_Proiect.Models.ToyShopViewModels
{
    public class BrandIndexData
    {
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Toy> Toys { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
