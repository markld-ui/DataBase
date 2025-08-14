using System.Configuration;

namespace Common
{
    /// <summary>
    /// Предоставляет методы для работы с конфигурацией приложения.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Этот класс является оберткой над стандартным <see cref="System.Configuration.ConfigurationManager"/>
    /// с дополнительной проверкой входных данных.
    /// </para>
    /// <para>
    /// Основное назначение - безопасное получение строк подключения из конфигурации.
    /// </para>
    /// </remarks>
    public static class ConfigurationManager
    {
        /// <summary>
        /// Получает строку подключения по указанному имени.
        /// </summary>
        /// <param name="name">Имя строки подключения в конфигурационном файле.</param>
        /// <returns>
        /// Найденная строка подключения.
        /// </returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        /// Выбрасывается, если строка подключения с указанным именем не найдена или пуста.
        /// </exception>
        /// <example>
        /// <code>
        /// string connectionString = ConfigurationManager.GetConnectionString("WarehouseConnection");
        /// </code>
        /// </example>
        /// <seealso cref="System.Configuration.ConfigurationManager.ConnectionStrings"/>
        public static string GetConnectionString(string name)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ConfigurationErrorsException($"Connection string '{name}' not found in App.config.");
            }

            return connectionString;
        }
    }
}