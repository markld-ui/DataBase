using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISupplierRepository
    {
        List<Supplier> GetAll();
        Supplier GetById(int id);
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(int id);
        List<Supplier> GetFiltered(string searchText);
    }
}
