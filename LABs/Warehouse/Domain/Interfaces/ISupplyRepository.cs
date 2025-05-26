using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISupplyRepository
    {
        List<Supply> GetAll();
        Supply GetById(int id);
        void Add(Supply supply);
        void Update(Supply supply);
        void Delete(int id);
        List<Supply> GetFiltered(string searchText);
    }
}
