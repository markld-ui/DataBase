using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Представляет модель зоны хранения в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс используется для хранения и передачи данных о зоне хранения, включая её идентификатор,
    /// идентификатор склада, вместимость, тип зоны и название зоны.
    /// </para>
    /// <para>
    /// Свойства класса соответствуют полям в базе данных и могут использоваться в операциях
    /// CRUD через репозитории.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var storageZone = new StorageZone
    /// {
    ///     StorageId = 1,
    ///     WarehouseId = 10,
    ///     Capacity = 1000,
    ///     ZoneType = ZoneType.Refrigerated,
    ///     ZoneName = "Холодильная зона A"
    /// };
    /// Console.WriteLine($"Зона: {storageZone.ZoneName}, Вместимость: {storageZone.Capacity}, Тип: {storageZone.ZoneType}");
    /// </code>
    /// </example>
    public class StorageZone
    {
        /// <summary>
        /// Получает или задаёт идентификатор зоны хранения.
        /// </summary>
        /// <remarks>
        /// Уникальный идентификатор зоны хранения в базе данных (первичный ключ).
        /// </remarks>
        public int StorageId { get; set; }

        /// <summary>
        /// Получает или задаёт идентификатор склада.
        /// </summary>
        /// <remarks>
        /// Ссылается на склад, к которому относится зона хранения (внешний ключ, соответствует <see cref="Warehouse.WarehouseId"/>).
        /// </remarks>
        public int WarehouseId { get; set; }

        /// <summary>
        /// Получает или задаёт вместимость зоны хранения.
        /// </summary>
        /// <remarks>
        /// Указывает максимальное количество единиц продукции, которое может быть размещено в зоне.
        /// </remarks>
        public int Capacity { get; set; }

        /// <summary>
        /// Получает или задаёт тип зоны хранения.
        /// </summary>
        /// <remarks>
        /// Определяет тип зоны, представленный перечислением <see cref="ZoneType"/> (например, холодильная, сухая).
        /// </remarks>
        public ZoneType ZoneType { get; set; }

        /// <summary>
        /// Получает или задаёт название зоны хранения.
        /// </summary>
        /// <remarks>
        /// Содержит название зоны (например, "Холодильная зона A"). Не должно быть null или пустым.
        /// </remarks>
        public string ZoneName { get; set; }
    }
}
