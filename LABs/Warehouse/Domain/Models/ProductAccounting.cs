using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductAccounting
    {
        public int ProductAccId { get; set; }
        public int SupplyId { get; set; }
        public int EmployeeId { get; set; }
        public int StorageId { get; set; }
        public DateTime AccountingDate { get; set; }
        public int Quantity { get; set; }
        public DateTime? LastMovementDate { get; set; }
        public string MovementStatus { get; set; } // Новое поле статуса движения
    }
}
