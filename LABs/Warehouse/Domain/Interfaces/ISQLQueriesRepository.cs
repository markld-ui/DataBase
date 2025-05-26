using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISQLQueriesRepository
    {
        DataTable GetAllRecords();
        DataTable GetRecordsByEmployee(int employeeId);
        DataTable GetAggregateRecords(int minRecordCount, DateTime startDate);
        DataTable GetSimpleProductAccountingRecords();
        DataTable GetCorrelatedSubquery(int supplyId);
        DataTable GetNonCorrelatedSubquery(int supplyId);
        bool EmployeeExists(int employeeId);
        bool SupplyExists(int supplyId);
        bool StorageZoneExists(int storageId);
        bool RecordExists(int recordId);
        void InsertRecord(DateTime accountingDate, int quantity, int employeeId, int supplyId, int storageId);
        void UpdateRecord(int id, DateTime? accountingDate, int? quantity, int? employeeId, int? supplyId, int? storageId);
        void DeleteRecord(int id);
    }
}
