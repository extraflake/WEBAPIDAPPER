using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication10.Model
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Stock { get; set; }
        public decimal Price { get; set; }
        public int Supplier_Id { get; set; }
    }
}
