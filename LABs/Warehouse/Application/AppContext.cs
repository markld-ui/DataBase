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
    /// <summary>
    /// Реализует паттерн Singleton для предоставления глобального доступа 
    /// к репозиториям приложения и подключению к базе данных.
    /// Является центральным классом, обеспечивающим единую точку доступа к данным.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует потокобезопасный Singleton с двойной проверкой блокировки.
    /// Все репозитории инициализируются при создании экземпляра AppContext.
    /// </para>
    /// <para>
    /// Подключение к базе данных создается на основе строки подключения из конфигурации.
    /// </para>
    /// </remarks>
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

        /// <summary>
        /// Приватный конструктор (реализация паттерна Singleton).
        /// Инициализирует подключение к базе данных и все репозитории.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        /// Возникает, если не удается получить строку подключения из конфигурации.
        /// </exception>
        /// <exception cref="System.Data.Common.DbException">
        /// Возникает при ошибках создания подключения к базе данных.
        /// </exception>
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

        /// <summary>
        /// Получает единственный экземпляр класса AppContext.
        /// </summary>
        /// <value>
        /// Единственный экземпляр класса AppContext.
        /// </value>
        /// <remarks>
        /// Реализация использует двойную проверку блокировки для потокобезопасности.
        /// </remarks>
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

        /// <summary>
        /// Получает репозиторий для работы с продуктами.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="IProductRepository"/>.
        /// </value>
        public IProductRepository ProductRepository => _productRepository;

        /// <summary>
        /// Получает репозиторий для работы с поставщиками.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="ISupplierRepository"/>.
        /// </value>
        public ISupplierRepository SupplierRepository => _supplierRepository;

        /// <summary>
        /// Получает репозиторий для работы с поставками.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="ISupplyRepository"/>.
        /// </value>
        public ISupplyRepository SupplyRepository => _supplyRepository;

        /// <summary>
        /// Получает репозиторий для работы с сотрудниками.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="IEmployeeRepository"/>.
        /// </value>
        public IEmployeeRepository EmployeeRepository => _employeeRepository;

        /// <summary>
        /// Получает репозиторий для работы со складами.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="IWarehouseRepository"/>.
        /// </value>
        public IWarehouseRepository WarehouseRepository => _warehouseRepository;

        /// <summary>
        /// Получает репозиторий для работы с зонами хранения.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="IStorageZoneRepository"/>.
        /// </value>
        public IStorageZoneRepository StorageZoneRepository => _storageZoneRepository;

        /// <summary>
        /// Получает репозиторий для учета продуктов.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="IProductAccountingRepository"/>.
        /// </value>
        public IProductAccountingRepository ProductAccountingRepository => _productAccountingRepository;

        /// <summary>
        /// Получает репозиторий для выполнения SQL-запросов.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="ISQLQueriesRepository"/>.
        /// </value>
        public ISQLQueriesRepository SQLQueriesRepository => _sqlQueriesRepository;

        /// <summary>
        /// Получает подключение к базе данных.
        /// </summary>
        /// <value>
        /// Экземпляр <see cref="DatabaseConnection"/>.
        /// </value>
        public DatabaseConnection DatabaseConnection => _dbConnection;
    }
}