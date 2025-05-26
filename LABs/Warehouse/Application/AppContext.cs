using System;
using Common;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Repositories;

namespace Application
{
    public class AppContext
    {
        private static AppContext _instance;
        private static readonly object _lock = new object();
        private readonly DatabaseConnection _dbConnection;
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplyRepository _supplyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IStorageZoneRepository _storageZoneRepository;
        private readonly IProductAccountingRepository _productAccountingRepository;
        private readonly ISQLQueriesRepository _sqlQueriesRepository;

        private AppContext()
        {
            string connectionString = Common.ConfigurationManager.GetConnectionString("WarehouseConnection");
            _dbConnection = new DatabaseConnection(connectionString);
            _productRepository = new ProductRepository(_dbConnection);
            _supplierRepository = new SupplierRepository(_dbConnection);
            _supplyRepository = new SupplyRepository(_dbConnection);
            _employeeRepository = new EmployeeRepository(_dbConnection);
            _warehouseRepository = new WarehouseRepository(_dbConnection);
            _storageZoneRepository = new StorageZoneRepository(_dbConnection);
            _productAccountingRepository = new ProductAccountingRepository(_dbConnection);
            _sqlQueriesRepository = new SQLQueriesRepository(_dbConnection);
        }

        public static AppContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AppContext();
                        }
                    }
                }
                return _instance;
            }
        }

        public IProductRepository ProductRepository => _productRepository;
        public ISupplierRepository SupplierRepository => _supplierRepository;
        public ISupplyRepository SupplyRepository => _supplyRepository;
        public IEmployeeRepository EmployeeRepository => _employeeRepository;
        public IWarehouseRepository WarehouseRepository => _warehouseRepository;
        public IStorageZoneRepository StorageZoneRepository => _storageZoneRepository;
        public IProductAccountingRepository ProductAccountingRepository => _productAccountingRepository;
        public ISQLQueriesRepository SQLQueriesRepository => _sqlQueriesRepository;
        public DatabaseConnection DatabaseConnection => _dbConnection;
    }
}
