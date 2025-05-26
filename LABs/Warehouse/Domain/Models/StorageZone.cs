using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StorageZone
    {
        public int StorageId { get; set; }
        public int WarehouseId { get; set; }
        public int Capacity { get; set; }
        public ZoneType ZoneType { get; set; }
        public string ZoneName { get; set; }
    }
}
