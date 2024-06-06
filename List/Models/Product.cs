using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace List.Models
{
    public class Product
    {
        [Key] public int ID { get; set; }
        public string ProductPhoto { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ManufacturerID { get; set; }
        public int QuantityInStock { get; set; }
    }
}
