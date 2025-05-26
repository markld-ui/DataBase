using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IStorageZoneRepository
    {
        List<StorageZone> GetAll();
        StorageZone GetById(int id);
        void Add(StorageZone storageZone);
        void Update(StorageZone storageZone);
        void Delete(int id);
        List<StorageZone> GetFiltered(string searchText);
    }
}
