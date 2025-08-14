using Npgsql;

namespace Infrastructure
{
    /// <summary>
    /// Предоставляет функциональность для управления подключением к базе данных PostgreSQL.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс инкапсулирует строку подключения и предоставляет методы для создания нового
    /// соединения с базой данных через библиотеку Npgsql, а также для проверки доступности базы данных.
    /// </para>
    /// <para>
    /// Экземпляры класса являются потокобезопасными, так как каждый вызов метода <see cref="GetConnection"/>
    /// создаёт новое соединение.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var dbConnection = new DatabaseConnection("Host=localhost;Username=user;Password=pass;Database=products");
    /// if (dbConnection.TestConnection())
    /// {
    ///     Console.WriteLine("Подключение к базе данных успешно!");
    /// }
    /// using (var conn = dbConnection.GetConnection())
    /// {
    ///     conn.Open();
    ///     // Выполнение запросов
    /// }
    /// </code>
    /// </example>
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DatabaseConnection"/>.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных PostgreSQL.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="connectionString"/> равен null.
        /// </exception>
        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Создаёт и возвращает новое соединение с базой данных PostgreSQL.
        /// </summary>
        /// <returns>
        /// Объект <see cref="NpgsqlConnection"/> для подключения к базе данных.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Каждый вызов метода создаёт новый экземпляр соединения, который должен быть
        /// закрыт или утилизирован вызывающей стороной после использования, предпочтительно
        /// с использованием конструкции using.
        /// </para>
        /// <para>
        /// Соединение возвращается в закрытом состоянии; его необходимо открыть перед использованием.
        /// </para>
        /// </remarks>
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        /// <summary>
        /// Проверяет возможность подключения к базе данных.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если подключение успешно; <c>false</c>, если подключение не удалось.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Метод пытается открыть соединение с базой данных. В случае ошибки подключения
        /// выводит сообщение об ошибке в консоль и возвращает <c>false</c>.
        /// </para>
        /// <para>
        /// Соединение автоматически закрывается после проверки.
        /// </para>
        /// </remarks>
        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                return false;
            }
        }
    }
}
