# Документация проекта: Система управления складом

## 1. Обзор проекта

**Система управления складом** — это Windows Forms приложение на C#, разработанное для управления данными о поставщиках, поставках, продуктах, сотрудниках, зонах хранения и складах. Приложение предоставляет графический интерфейс для выполнения операций CRUD (создание, чтение, обновление, удаление), фильтрации и навигации по данным. Проект использует PostgreSQL как базу данных и реализует многослойную архитектуру с разделением UI, домена, инфраструктуры и общего слоя (`Common`).

### 1.1 Цели проекта
- Обеспечить удобный интерфейс для управления складскими операциями.
- Реализовать CRUD-операции для всех сущностей системы.
- Поддерживать фильтрацию и поиск данных.
- Обеспечить интеграцию с PostgreSQL через кастомные репозитории.
- Обеспечить масштабируемость и поддержку многослойной архитектуры.

### 1.2 Основные возможности
- Управление поставщиками, продуктами, поставками, сотрудниками, зонами хранения и складами.
- Отображение данных в таблицах с использованием `DataGridView` и навигации через `BindingNavigator`.
- Фильтрация данных по текстовому запросу.
- Валидация пользовательского ввода на уровне форм.
- Подключение к PostgreSQL через `DatabaseConnection`.

### 1.3 Предполагаемое использование
Приложение ориентировано на складских менеджеров, сотрудников и администраторов для:
- Ведения учёта поставщиков и их контактов.
- Регистрации поставок и учёта продукции.
- Управления складскими зонами и сотрудниками.
- Анализа складских операций.

## 2. Архитектура проекта

Проект использует многослойную архитектуру с разделением ответственности.

### 2.1 Слои
1. **UI (Пользовательский интерфейс)**:
   - Пространство имён: `UI`.
   - Содержит формы: `AboutForm`, `EmployeeForm`, `MainForm`, `ProductAccountingForm`, `ProductForm`, `StorageZoneForm`, `SupplierForm`.
   - Отвечает за отображение и взаимодействие с пользователем.
2. **Domain (Доменная логика)**:
   - Пространства имён: `Domain.Interfaces`, `Domain.Models`.
   - Содержит интерфейсы репозиториев и модели данных.
3. **Infrastructure (Инфраструктура)**:
   - Пространства имён: `Infrastructure.Repositories`, `Infrastructure.DatabaseConnection`.
   - Содержит реализацию репозиториев и подключение к базе данных.
4. **Application**:
   - Пространства имён: `Application`, `Application.Common`.
   - Содержит `AppContext` для управления репозиториями и утилиты.
5. **Common (Общий слой)**:
   - Пространство имён: `Application.Common`.
   - Содержит утилитарные классы, такие как `ConfigurationManager`, для управления конфигурацией приложения (например, строкой подключения к базе данных).
   - Обеспечивает централизованный доступ к настройкам, что упрощает конфигурирование и поддержку.

### 2.2 Зависимости
- **.NET Framework**: 4.8 (или выше) для Windows Forms.
- **Npgsql**: Драйвер для подключения к PostgreSQL.
- **System.Data**: Для работы с ADO.NET.
- **System.Windows.Forms**: Для UI.
- **System.ComponentModel**: Для `BindingSource`.

### 2.3 Предполагаемая структура файлов
- **Application**:
  - `AppContext.cs`: Синглтон для доступа к репозиториям.
  - `Common/ConfigurationManager.cs`: Управление конфигурацией (например, строкой подключения).
- **Domain**:
  - `Interfaces/*.cs`: Интерфейсы репозиториев.
  - `Models/*.cs`: Модели данных (например, `Supplier`, `Supply`).
- **Infrastructure**:
  - `Repositories/*.cs`: Реализации репозиториев (например, `SupplierRepository`).
  - `DatabaseConnection.cs`: Подключение к PostgreSQL.
- **UI**:
  - `AboutForm.cs`, `EmployeeForm.cs`, `MainForm.cs`, `ProductAccountingForm.cs`, `ProductForm.cs`, `StorageZoneForm.cs`, `SupplierForm.cs`.
  - `App.config`: Конфигурационный файл.
  - `logo.png`: Логотип приложения.
  - `Program.cs`: Точка входа.
  - `Settings.settings`: Настройки приложения.
- **SQLQueries.cs**: SQL-запросы для базы данных.

## 3. Подробное описание файлов

### 3.1 Application
- **AppContext.cs**:
  - **Описание**: Синглтон для предоставления экземпляров репозиториев.
  - **Поля**:
    - `Instance: AppContext` — статический экземпляр.
    - Репозитории (например, `ISupplierRepository SupplierRepository`, `IProductRepository ProductRepository`, `ISupplyRepository SupplyRepository`, `IEmployeeRepository EmployeeRepository`, `IStorageZoneRepository StorageZoneRepository`, `IWarehouseRepository WarehouseRepository`).
  - **Методы**:
    - `Instance`: Свойство для получения единственного экземпляра.
    - Конструктор: Инициализирует репозитории с использованием `ConfigurationManager`.
  - **Особенности**: Использует `ConfigurationManager` для получения строки подключения и других настроек. Обеспечивает централизованный доступ ко всем репозиториям, что упрощает внедрение зависимостей в формы.
  - **Пример использования**:
    ```csharp
    var supplierRepo = AppContext.Instance.SupplierRepository;
    var suppliers = supplierRepo.GetAll();
    ```

- **Common/ConfigurationManager.cs**:
  - **Описание**: Утилитарный класс для управления конфигурацией приложения, включая строку подключения к PostgreSQL и другие настройки.
  - **Поля**:
    - `ConnectionString: string` — строка подключения к базе данных, загружаемая из `App.config`.
    - `Settings: Dictionary<string, string>` — словарь для хранения дополнительных настроек (например, языка интерфейса или параметров логирования).
  - **Методы**:
    - `GetConnectionString(): string`: Возвращает строку подключения из `App.config`.
    - `LoadSettings(): void`: Загружает дополнительные настройки из конфигурационного файла или других источников (например, `Settings.settings`).
    - `GetSetting(string key): string`: Возвращает значение настройки по ключу.
  - **Особенности**:
    - Использует `System.Configuration.ConfigurationManager` для работы с `App.config`.
    - Централизует управление конфигурацией, что упрощает изменение настроек без необходимости правки кода.
    - Может быть расширен для поддержки других источников конфигурации (например, JSON-файлов или переменных окружения).
  - **Пример использования**:
    ```csharp
    var connectionString = ConfigurationManager.GetConnectionString();
    var language = ConfigurationManager.GetSetting("ApplicationLanguage");
    ```

### 3.2 Domain
- **Interfaces/*.cs**:
  - **Описание**: Определяют контракты для репозиториев.
  - **Пример (ISupplierRepository.cs)**:
    ```csharp
    public interface ISupplierRepository
    {
        List<Supplier> GetAll();
        List<Supplier> GetFiltered(string searchText);
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(int supplierId);
    }
    ```
  - **Другие интерфейсы**:
    - `IProductRepository`, `ISupplyRepository`, `IEmployeeRepository`, `IStorageZoneRepository`, `IWarehouseRepository`: Аналогичны, с методами `GetAll`, `GetFiltered`, `Add`, `Update`, `Delete`.
  - **Особенности**: Интерфейсы унифицированы для всех сущностей, что обеспечивает единообразие взаимодействия между слоями.

- **Models/*.cs**:
  - **Supplier.cs**:
    ```csharp
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
    ```
  - **Product.cs**:
    ```csharp
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime? ShelfLife { get; set; }
        public string ProductType { get; set; }
    }
    ```
  - **Supply.cs**:
    ```csharp
    public class Supply
    {
        public int SupplyId { get; set; }
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public DateTime SupplyDate { get; set; }
        public int Quantity { get; set; }
        public int? DeliveredQuantity { get; set; }
    }
    ```
  - **Employee.cs**:
    ```csharp
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
    }
    ```
  - **ProductAccounting.cs**:
    ```csharp
    public class ProductAccounting
    {
        public int ProductAccId { get; set; }
        public int SupplyId { get; set; }
        public int EmployeeId { get; set; }
        public int StorageId { get; set; }
        public DateTime AccountDate { get; set; }
        public int Quantity { get; set; }
        public DateTime? LastMovementDate { get; set; }
    }
    ```
  - **StorageZone.cs**:
    ```csharp
    public class StorageZone
    {
        public int StorageId { get; set; }
        public int WarehouseId { get; set; }
        public int? Height { get; set; }
        public string ZoneType { get; set; }
        public string ZoneName { get; set; }
    }
    ```
  - **Warehouse.cs**:
    ```csharp
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
    ```
  - **Особенности**: Модели соответствуют IDEF1X диаграмме, с учётом всех полей и их типов.

### 3.3 Infrastructure
- **Repositories/*.cs**:
  - **Описание**: Реализации репозиториев с использованием `DatabaseConnection`.
  - **Пример (SupplierRepository.cs)**:
    ```csharp
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DatabaseConnection _db;
        public SupplierRepository() => _db = new DatabaseConnection();
        public List<Supplier> GetAll() => _db.ExecuteQuery<Supplier>("SELECT * FROM suppliers");
        public void Add(Supplier supplier) => _db.ExecuteNonQuery("INSERT INTO suppliers ...", supplier);
    }
    ```
- **DatabaseConnection.cs**:
  - **Описание**: Управляет подключением к PostgreSQL.
  - **Поля**:
    - `Connection: NpgsqlConnection` — подключение.
  - **Методы**:
    - `ExecuteQuery<T>`: Выполняет SELECT-запросы.
    - `ExecuteNonQuery`: Выполняет INSERT/UPDATE/DELETE.
  - **Особенности**: Использует `ConfigurationManager.GetConnectionString()`.

### 3.4 UI
- **AboutForm.cs**: Форма с информацией о приложении.
- **EmployeeForm.cs**: Управление сотрудниками.
- **MainForm.cs**: Главная форма с меню.
- **ProductAccountingForm.cs**: Учёт продукции.
- **ProductForm.cs**: Управление продуктами.
- **StorageZoneForm.cs**: Управление зонами хранения.
- **SupplierForm.cs**: Управление поставщиками (см. предыдущую документацию).
- **App.config**: Содержит строку подключения.
- **logo.png**: Логотип.
- **Program.cs**: Точка входа.
- **Settings.settings**: Настройки.
- **SQLQueries.cs**: SQL-запросы.

## 4. База данных (PostgreSQL)

### 4.1 IDEF1X Диаграмма
- **Сущности**:
  - **Товары (product)**: `product_id`, `наименование`, `срок годности`, `тип товара`.
  - **Поставщики (supplier)**: `supplier_id`, `наименование компании`, `контактное лицо`, `телефон`, `адрес`.
  - **Поставка (supply)**: `supply_id`, `product_id`, `supplier_id`, `дата поставки`, `количество`, `поставленное количество`.
  - **Сотрудник (employee)**: `employee_id`, `фио`, `должность`, `номер телефона`.
  - **Учёт товара (product_accounting)**: `product_acc_id`, `supply_id`, `employee_id`, `storage_id`, `дата учёта`, `количество`, `дата последнего перемещения`.
  - **Зона хранения (storage_zone)**: `storage_id`, `warehouse_id`, `высота`, `тип зоны`, `наименование зоны`.
  - **Склад (warehouse)**: `warehouse_id`, `наименование`, `адрес`.

### 4.2 Полная SQL Схема
Ниже приведена полная SQL-схема для создания таблиц в PostgreSQL с учётом IDEF1X диаграммы. Добавлены ограничения на ключи, типы данных и значения по умолчанию, где это уместно.

```sql
-- Таблица поставщиков
CREATE TABLE suppliers (
    supplier_id SERIAL PRIMARY KEY,
    company_name VARCHAR(100) NOT NULL,
    contact_person VARCHAR(100),
    phone VARCHAR(20),
    address VARCHAR(200),
    CONSTRAINT chk_company_name CHECK (company_name <> '')
);

-- Таблица товаров
CREATE TABLE products (
    product_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    shelf_life DATE,
    product_type VARCHAR(50),
    CONSTRAINT chk_name CHECK (name <> '')
);

-- Таблица поставок
CREATE TABLE supplies (
    supply_id SERIAL PRIMARY KEY,
    product_id INT NOT NULL,
    supplier_id INT NOT NULL,
    supply_date DATE NOT NULL DEFAULT CURRENT_DATE,
    quantity INT NOT NULL,
    delivered_quantity INT,
    CONSTRAINT fk_supply_product FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE RESTRICT,
    CONSTRAINT fk_supply_supplier FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id) ON DELETE RESTRICT,
    CONSTRAINT chk_quantity CHECK (quantity > 0),
    CONSTRAINT chk_delivered_quantity CHECK (delivered_quantity IS NULL OR delivered_quantity >= 0)
);

-- Таблица сотрудников
CREATE TABLE employees (
    employee_id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    position VARCHAR(50),
    phone_number VARCHAR(20),
    CONSTRAINT chk_full_name CHECK (full_name <> '')
);

-- Таблица зон хранения
CREATE TABLE storage_zones (
    storage_id SERIAL PRIMARY KEY,
    warehouse_id INT NOT NULL,
    height INT,
    zone_type VARCHAR(50),
    zone_name VARCHAR(100) NOT NULL,
    CONSTRAINT fk_storage_zone_warehouse FOREIGN KEY (warehouse_id) REFERENCES warehouses(warehouse_id) ON DELETE CASCADE,
    CONSTRAINT chk_height CHECK (height IS NULL OR height > 0),
    CONSTRAINT chk_zone_name CHECK (zone_name <> '')
);

-- Таблица складов
CREATE TABLE warehouses (
    warehouse_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    address VARCHAR(200),
    CONSTRAINT chk_name CHECK (name <> '')
);

-- Таблица учёта товаров
CREATE TABLE product_accounting (
    product_acc_id SERIAL PRIMARY KEY,
    supply_id INT NOT NULL,
    employee_id INT NOT NULL,
    storage_id INT NOT NULL,
    account_date DATE NOT NULL DEFAULT CURRENT_DATE,
    quantity INT NOT NULL,
    last_movement_date DATE,
    CONSTRAINT fk_product_accounting_supply FOREIGN KEY (supply_id) REFERENCES supplies(supply_id) ON DELETE RESTRICT,
    CONSTRAINT fk_product_accounting_employee FOREIGN KEY (employee_id) REFERENCES employees(employee_id) ON DELETE RESTRICT,
    CONSTRAINT fk_product_accounting_storage FOREIGN KEY (storage_id) REFERENCES storage_zones(storage_id) ON DELETE RESTRICT,
    CONSTRAINT chk_quantity CHECK (quantity >= 0)
);

-- Индексы для оптимизации поиска
CREATE INDEX idx_supplies_product_id ON supplies(product_id);
CREATE INDEX idx_supplies_supplier_id ON supplies(supplier_id);
CREATE INDEX idx_product_accounting_supply_id ON product_accounting(supply_id);
CREATE INDEX idx_storage_zones_warehouse_id ON storage_zones(warehouse_id);
```

### 4.3 Комментарии к схеме
- **Ограничения**:
  - Поля с `NOT NULL` (например, `company_name`, `full_name`, `quantity`) гарантируют, что ключевые данные всегда заполнены.
  - Проверки (`CHECK`) добавлены для обеспечения корректности данных (например, `quantity > 0`).
  - Внешние ключи (`FOREIGN KEY`) обеспечивают целостность связей.
  - `ON DELETE RESTRICT` предотвращает удаление записей, на которые ссылаются другие таблицы.
  - `ON DELETE CASCADE` в `storage_zones` позволяет автоматически удалять зоны хранения при удалении склада.
- **Индексы**:
  - Добавлены индексы на часто используемые поля для ускорения запросов.
- **Типы данных**:
  - `SERIAL` используется для автоинкрементных первичных ключей.
  - `VARCHAR` для строковых полей с разумными ограничениями длины.
  - `DATE` для дат, с значением по умолчанию `CURRENT_DATE` где уместно.

## 5. Установка и настройка

### 5.1 Требования
- **ОС**: Windows 7/10/11.
- **.NET Framework**: 4.8.
- **PostgreSQL**: 12.x или выше.
- **Npgsql**: Установить через NuGet.

### 5.2 Установка
1. Установите PostgreSQL и создайте базу данных.
2. Выполните SQL-схему для создания таблиц.
3. Добавьте Npgsql через NuGet: `Install-Package Npgsql`.
4. Откройте проект в Visual Studio.
5. Настройте `App.config`:
   ```xml
   <connectionStrings>
       <add name="WarehouseConnection" connectionString="Host=localhost;Database=warehouse;Username=postgres;Password=your_password" />
   </connectionStrings>
   ```
6. Скомпилируйте проект.

### 5.3 Конфигурация
- Убедитесь, что `ConfigurationManager.cs` корректно загружает строку подключения из `App.config`.
- Проверьте настройки в `Settings.settings` (например, язык интерфейса).

## 6. Использование
- Запустите приложение через `MainForm.cs`.
- Используйте меню для вызова форм (например, `SupplierForm`).

## 7. Тестирование
- **Ручное**: Проверьте CRUD и фильтрацию на каждой форме.
- **Автоматизированное**: Используйте MSTest с моками для репозиториев.

## 8. Развертывание
- Скомпилируйте в Release.
- Установите зависимости на целевой машине.
- Разверните с настройками БД.

## 9. Замечания и рекомендации
- **Ограничения**: Минимальная валидация, отсутствие логирования.
- **Рекомендации**:
  - Добавить валидацию (например, формат телефона, диапазон дат).
  - Внедрить логирование (например, Serilog).
  - Добавить юнит-тесты для репозиториев.
  - Оптимизировать запросы с помощью дополнительных индексов.

## 10. Лицензия
Учебный проект.

## 11. Контакты
Свяжитесь через репозиторий.

---
*Создано: 27 мая 2025 года, 04:32 CEST*