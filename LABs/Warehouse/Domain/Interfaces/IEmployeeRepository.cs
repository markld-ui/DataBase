using Domain.Models;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();
        Employee GetById(int id);
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(int id);
        List<Employee> GetFiltered(string searchText);
    }
}