# Документация проекта: Система управления складом

## 1. Обзор проекта

**Система управления складом** — это Windows Forms приложение на C#, разработанное для управления данными о поставщиках, поставках, продуктах, сотрудниках, зонах хранения и складах. Приложение предоставляет графический интерфейс для выполнения операций CRUD (создание, чтение, обновление, удаление), фильтрации и навигации по данным. Проект использует PostgreSQL как базу данных и реализует многослойную архитектуру с разделением UI, домена и инфраструктуры.

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
   - Содержит `AppContext` и утилиты (`ConfigurationManager`).

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
    - Репозитории (например, `ISupplierRepository`, `IProductRepository`).
  - **Методы**:
    - `Instance`: Свойство для получения единственного экземпляра.
    - Конструктор: Инициализирует репозитории.
  - **Особенности**: Использует `ConfigurationManager` для настройки.
- **Common/ConfigurationManager.cs**:
  - **Описание**: Управляет конфигурацией, включая строку подключения к PostgreSQL.
  - **Методы**:
    - `GetConnectionString()`: Возвращает строку подключения.
  - **Особенности**: Читает данные из `App.config`.

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
  - **Особенности**: Интерфейсы унифицированы для всех сущностей.
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
  - **Supply.cs**, **Product.cs**, **Employee.cs**, **StorageZone.cs**, **Warehouse.cs**: Аналогично, с полями согласно IDEF1X.

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
- **AboutForm.cs**:
  - **Описание**: Форма с информацией о приложении.
  - **Особенности**: Содержит текст о версии и разработчике.
- **EmployeeForm.cs**:
  - **Описание**: Управление сотрудниками.
  - **Поля**: `_employeeRepository`, `_selectedEmployee`, `_bindingSource`.
  - **Методы**: CRUD-операции, фильтрация.
- **MainForm.cs**:
  - **Описание**: Главная форма с меню для вызова других форм.
  - **Особенности**: Содержит кнопки или меню для навигации.
- **ProductAccountingForm.cs**:
  - **Описание**: Учёт продукции (аналогично `SupplyForm`).
  - **Поля**: `_productAccountingRepository`.
- **ProductForm.cs**:
  - **Описание**: Управление продуктами.
  - **Поля**: `_productRepository`.
- **StorageZoneForm.cs**:
  - **Описание**: Управление зонами хранения.
  - **Поля**: `_storageZoneRepository`.
- **SupplierForm.cs**:
  - **Описание**: Управление поставщиками (см. предыдущую документацию).
- **App.config**:
  - **Описание**: Содержит строку подключения (`connectionStrings`).
- **logo.png**:
  - **Описание**: Логотип приложения.
- **Program.cs**:
  - **Описание**: Точка входа, запускает `MainForm`.
  - **Метод**: `Main`: Создаёт экземпляр `Application`.
- **Settings.settings**:
  - **Описание**: Хранит настройки (например, язык интерфейса).
- **SQLQueries.cs**:
  - **Описание**: Содержит SQL-запросы для PostgreSQL.
  - **Пример**:
    ```csharp
    public static class SQLQueries
    {
        public const string GetSuppliers = "SELECT * FROM suppliers WHERE company_name LIKE @search";
    }
    ```

## 4. База данных (PostgreSQL)

### 4.1 IDEF1X Диаграмма
- **Сущности**:
  - **Товары (product)**:
    - `product_id` (PK).
    - `наименование`, `срок годности`, `тип товара`.
  - **Поставщики (supplier)**:
    - `supplier_id` (PK).
    - `наименование компании`, `контактное лицо`, `телефон`, `адрес`.
  - **Поставка (supply)**:
    - `supply_id` (PK).
    - `product_id` (FK), `supplier_id` (FK).
    - `дата поставки`, `количество`, `поставленное количество`.
  - **Сотрудник (employee)**:
    - `employee_id` (PK).
    - `фио`, `должность`, `номер телефона`.
  - **Учёт товара (product_accounting)**:
    - `product_acc_id` (PK).
    - `supply_id` (FK), `employee_id` (FK), `storage_id` (FK).
    - `дата учёта`, `количество`, `дата последнего перемещения`.
  - **Зона хранения (storage_zone)**:
    - `storage_id` (PK).
    - `warehouse_id` (FK).
    - `высота`, `тип зоны`, `наименование зоны`.
  - **Склад (warehouse)**:
    - `warehouse_id` (PK).
    - `наименование`, `адрес`.

### 4.2 SQL Схема
```sql
CREATE TABLE suppliers (
    supplier_id SERIAL PRIMARY KEY,
    company_name VARCHAR(100) NOT NULL,
    contact_person VARCHAR(100),
    phone VARCHAR(20),
    address VARCHAR(200)
);

CREATE TABLE products (
    product_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    shelf_life DATE,
    product_type VARCHAR(50)
);

CREATE TABLE supplies (
    supply_id SERIAL PRIMARY KEY,
    product_id INT REFERENCES products(product_id),
    supplier_id INT REFERENCES suppliers(supplier_id),
    supply_date DATE NOT NULL,
    quantity INT NOT NULL,
    delivered_quantity INT
);

CREATE TABLE employees (
    employee_id SERIAL PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    position VARCHAR(50),
    phone_number VARCHAR(20)
);

CREATE TABLE product_accounting (
    product_acc_id SERIAL PRIMARY KEY,
    supply_id INT REFERENCES supplies(supply_id),
    employee_id INT REFERENCES employees(employee_id),
    storage_id INT REFERENCES storage_zones(storage_id),
    account_date DATE NOT NULL,
    quantity INT NOT NULL,
    last_movement_date DATE
);

CREATE TABLE storage_zones (
    storage_id SERIAL PRIMARY KEY,
    warehouse_id INT REFERENCES warehouses(warehouse_id),
    height INT,
    zone_type VARCHAR(50),
    zone_name VARCHAR(100)
);

CREATE TABLE warehouses (
    warehouse_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    address VARCHAR(200)
);
```

## 5. Установка и настройка

### 5.1 Требования
- **ОС**: Windows 7/10/11.
- **.NET Framework**: 4.8.
- **PostgreSQL**: 12.x или выше.
- **IDE**: Visual Studio 2019/2022.
- **Npgsql**: Установить через NuGet.

### 5.2 Установка
1. Установите PostgreSQL и создайте базу данных.
2. Добавьте Npgsql через NuGet: `Install-Package Npgsql`.
3. Откройте проект в Visual Studio.
4. Настройте `App.config`:
   ```xml
   <connectionStrings>
       <add name="WarehouseConnection" connectionString="Host=localhost;Database=warehouse;Username=postgres;Password=your_password" />
   </connectionStrings>
   ```
5. Скомпилируйте проект.

### 5.3 Конфигурация
- Обновите `ConfigurationManager.cs` с актуальной строкой подключения.
- Создайте таблицы в PostgreSQL с помощью приведённой схемы.

## 6. Использование
- Запустите через `MainForm.cs`.
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
- **Рекомендации**: Добавить валидацию, логирование, тесты.

## 10. Лицензия
Учебный проект.

## 11. Контакты
Свяжитесь через репозиторий.

---
*Создано: 27 мая 2025 года, 04:16 CEST*