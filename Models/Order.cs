using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pascu_Serban_Proiect.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int ToyID { get; set; }
        public int WorkerID { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public Worker Worker { get; set; }
        public Toy Toy { get; set; }
    }
}
