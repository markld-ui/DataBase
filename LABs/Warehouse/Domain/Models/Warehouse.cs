using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Представляет модель склада в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс используется для хранения и передачи данных о складах,
    /// включая их идентификаторы, названия и адреса.
    /// </para>
    /// <para>
    /// Свойства класса соответствуют полям в базе данных и используются
    /// в операциях CRUD через соответствующие репозитории.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var warehouse = new Warehouse
    /// {
    ///     WarehouseId = 1,
    ///     Name = "Основной склад",
    ///     Address = "г. Москва, ул. Складская, д. 1"
    /// };
    /// Console.WriteLine($"Склад: {warehouse.Name}, Адрес: {warehouse.Address}");
    /// </code>
    /// </example>
    public class Warehouse
    {
        /// <summary>
        /// Получает или задает уникальный идентификатор склада.
        /// </summary>
        /// <remarks>
        /// Первичный ключ для записи о складе в базе данных.
        /// </remarks>
        public int WarehouseId { get; set; }

        /// <summary>
        /// Получает или задает название склада.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает физический адрес склада.
        /// </summary>
        public string Address { get; set; }
    }
}