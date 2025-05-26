using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IWarehouseRepository
    {
        List<Warehouse> GetAll();
        Warehouse GetById(int id);
        void Add(Warehouse warehouse);
        void Update(Warehouse warehouse);
        void Delete(int id);
        List<Warehouse> GetFiltered(string searchText);
    }
}
