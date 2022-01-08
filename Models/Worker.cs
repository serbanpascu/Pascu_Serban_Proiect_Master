using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pascu_Serban_Proiect.Models
{
    public class Worker
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WorkerID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
