using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IProductAccountingRepository
    {
        List<ProductAccounting> GetAll();
        ProductAccounting GetById(int id);
        void Add(ProductAccounting productAccounting);
        void Update(ProductAccounting productAccounting);
        void Delete(int id);
        List<ProductAccounting> GetFiltered(string searchText);
    }
}
