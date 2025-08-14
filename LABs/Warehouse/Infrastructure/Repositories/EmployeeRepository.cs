using Npgsql;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с данными сотрудников в базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс реализует интерфейс <see cref="IEmployeeRepository"/> и предоставляет методы для выполнения CRUD-операций
    /// и фильтрации данных сотрудников с использованием PostgreSQL через библиотеку Npgsql.
    /// </para>
    /// <para>
    /// Все методы являются потокобезопасными, так как используют отдельные подключения к базе данных,
    /// создаваемые через <see cref="DatabaseConnection"/>.
    /// </para>
    /// <para>
    /// Для корректной работы требуется настроенное подключение к базе данных PostgreSQL.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var dbConnection = new DatabaseConnection("Host=localhost;Username=user;Password=pass;Database=employees");
    /// var repository = new EmployeeRepository(dbConnection);
    /// var employees = repository.GetAll();
    /// foreach (var employee in employees)
    /// {
    ///     Console.WriteLine($"ID: {employee.EmployeeId}, Name: {employee.FullName}, Position: {employee.Position}");
    /// }
    /// </code>
    /// </example>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseConnection _dbConnection;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EmployeeRepository"/>.
        /// </summary>
        /// <param name="dbConnection">Объект подключения к базе данных типа <see cref="DatabaseConnection"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="dbConnection"/> равен null.
        /// </exception>
        public EmployeeRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Получает список всех сотрудников из базы данных.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{Employee}"/>, содержащий всех сотрудников.
        /// Возвращает пустой список, если сотрудники отсутствуют.
        /// </returns>
        /// <remarks>
        /// Метод выполняет SQL-запрос для получения всех записей из таблицы employee.
        /// </remarks>
        public List<Employee> GetAll()
        {
            var employees = new List<Employee>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT employee_id, photo, full_name, position, phone FROM employee", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeId = reader.GetInt32(0),
                            Photo = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1),
                            FullName = reader.GetString(2),
                            Position = reader.GetString(3),
                            Phone = reader.IsDBNull(4) ? null : reader.GetString(4)
                        });
                    }
                }
            }

            return employees;
        }

        /// <summary>
        /// Получает сотрудника по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника (employee_id).</param>
        /// <returns>
        /// Объект <see cref="Employee"/> с данными сотрудника или null, если сотрудник не найден.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен нулю.
        /// </exception>
        public Employee GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT employee_id, photo, full_name, position, phone FROM employee WHERE employee_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                EmployeeId = reader.GetInt32(0),
                                Photo = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1),
                                FullName = reader.GetString(2),
                                Position = reader.GetString(3),
                                Phone = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет нового сотрудника в базу данных.
        /// </summary>
        /// <param name="employee">Объект сотрудника типа <see cref="Employee"/> для добавления.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="employee"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если обязательные поля <see cref="Employee.FullName"/> или <see cref="Employee.Position"/> пусты.
        /// </exception>
        /// <remarks>
        /// Метод устанавливает свойство <see cref="Employee.EmployeeId"/> на основе возвращаемого значения SQL-запроса.
        /// </remarks>
        public void Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (string.IsNullOrWhiteSpace(employee.FullName))
                throw new ArgumentException("Полное имя сотрудника не может быть пустым.", nameof(employee.FullName));

            if (string.IsNullOrWhiteSpace(employee.Position))
                throw new ArgumentException("Должность сотрудника не может быть пустой.", nameof(employee.Position));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO employee (photo, full_name, position, phone) VALUES (@photo, @full_name, @position, @phone) RETURNING employee_id", conn))
                {
                    cmd.Parameters.AddWithValue("photo", (object)employee.Photo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("full_name", employee.FullName);
                    cmd.Parameters.AddWithValue("position", employee.Position);
                    cmd.Parameters.AddWithValue("phone", (object)employee.Phone ?? DBNull.Value);
                    employee.EmployeeId = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Обновляет данные существующего сотрудника в базе данных.
        /// </summary>
        /// <param name="employee">Объект сотрудника типа <see cref="Employee"/> с обновлёнными данными.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="employee"/> равен null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <see cref="Employee.EmployeeId"/> меньше или равен нулю, либо
        /// если обязательные поля <see cref="Employee.FullName"/> или <see cref="Employee.Position"/> пусты.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если сотрудник с указанным <see cref="Employee.EmployeeId"/> не найден в базе данных.
        /// </exception>
        public void Update(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (employee.EmployeeId <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(employee.EmployeeId));

            if (string.IsNullOrWhiteSpace(employee.FullName))
                throw new ArgumentException("Полное имя сотрудника не может быть пустым.", nameof(employee.FullName));

            if (string.IsNullOrWhiteSpace(employee.Position))
                throw new ArgumentException("Должность сотрудника не может быть пустой.", nameof(employee.Position));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "UPDATE employee SET photo = @photo, full_name = @full_name, position = @position, phone = @phone WHERE employee_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", employee.EmployeeId);
                    cmd.Parameters.AddWithValue("photo", (object)employee.Photo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("full_name", employee.FullName);
                    cmd.Parameters.AddWithValue("position", employee.Position);
                    cmd.Parameters.AddWithValue("phone", (object)employee.Phone ?? DBNull.Value);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Сотрудник с идентификатором {employee.EmployeeId} не найден.");
                }
            }
        }

        /// <summary>
        /// Удаляет сотрудника из базы данных по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника (employee_id).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен нулю.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если сотрудник с указанным идентификатором не найден.
        /// </exception>
        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Идентификатор сотрудника должен быть больше нуля.", nameof(id));

            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("DELETE FROM employee WHERE employee_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        throw new KeyNotFoundException($"Сотрудник с идентификатором {id} не найден.");
                }
            }
        }

        /// <summary>
        /// Получает отфильтрованный список сотрудников на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по идентификатору, имени, должности или телефону).</param>
        /// <returns>
        /// Список <see cref="List{Employee}"/>, содержащий сотрудников, удовлетворяющих условиям поиска.
        /// Возвращает пустой список, если сотрудники не найдены.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Поиск выполняется без учёта регистра с использованием оператора ILIKE в PostgreSQL.
        /// </para>
        /// <para>
        /// Если параметр <paramref name="searchText"/> равен null или пустой строке, метод возвращает пустой список.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var repository = new EmployeeRepository(dbConnection);
        /// var filteredEmployees = repository.GetFiltered("manager");
        /// foreach (var employee in filteredEmployees)
        /// {
        ///     Console.WriteLine($"ID: {employee.EmployeeId}, Name: {employee.FullName}");
        /// }
        /// </code>
        /// </example>
        public List<Employee> GetFiltered(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<Employee>();

            var employees = new List<Employee>();
            using (var conn = _dbConnection.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT employee_id, photo, full_name, position, phone
                    FROM employee
                    WHERE employee_id::text ILIKE @search
                    OR full_name ILIKE @search
                    OR position ILIKE @search
                    OR phone ILIKE @search";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("search", $"%{searchText}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = reader.GetInt32(0),
                                Photo = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1),
                                FullName = reader.GetString(2),
                                Position = reader.GetString(3),
                                Phone = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return employees;
        }
    }
}