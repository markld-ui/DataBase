using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public ProductType ProductType { get; set; }
        public bool IsActive { get; set; }
        public byte[] Photo { get; set; }
    }
}
