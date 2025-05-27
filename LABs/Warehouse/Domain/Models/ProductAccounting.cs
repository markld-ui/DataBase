using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Представляет модель учёта продукции в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс используется для хранения и передачи данных об учёте продукции, включая идентификатор записи,
    /// информацию о поставке, сотруднике, зоне хранения, дате учёта, количестве, дате последнего движения
    /// и статусе движения.
    /// </para>
    /// <para>
    /// Свойства класса соответствуют полям в базе данных и могут использоваться в операциях
    /// CRUD через репозитории.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var productAccounting = new ProductAccounting
    /// {
    ///     ProductAccId = 1,
    ///     SupplyId = 100,
    ///     EmployeeId = 10,
    ///     StorageId = 5,
    ///     AccountingDate = DateTime.Now,
    ///     Quantity = 50,
    ///     LastMovementDate = DateTime.Now.AddDays(-1),
    ///     MovementStatus = "На складе"
    /// };
    /// Console.WriteLine($"Учёт: ID {productAccounting.ProductAccId}, Количество: {productAccounting.Quantity}, Статус: {productAccounting.MovementStatus}");
    /// </code>
    /// </example>
    public class ProductAccounting
    {
        /// <summary>
        /// Получает или задаёт идентификатор записи учёта продукции.
        /// </summary>
        /// <remarks>
        /// Уникальный идентификатор записи в базе данных (первичный ключ).
        /// </remarks>
        public int ProductAccId { get; set; }

        /// <summary>
        /// Получает или задаёт идентификатор поставки.
        /// </summary>
        /// <remarks>
        /// Ссылается на связанную поставку (внешний ключ, соответствует <see cref="Supply.SupplyId"/>).
        /// </remarks>
        public int SupplyId { get; set; }

        /// <summary>
        /// Получает или задаёт идентификатор сотрудника.
        /// </summary>
        /// <remarks>
        /// Ссылается на сотрудника, ответственного за учёт (внешний ключ, соответствует <see cref="Employee.EmployeeId"/>).
        /// </remarks>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Получает или задаёт идентификатор зоны хранения.
        /// </summary>
        /// <remarks>
        /// Ссылается на зону хранения, где находится продукция (внешний ключ, соответствует <see cref="StorageZone.StorageId"/>).
        /// </remarks>
        public int StorageId { get; set; }

        /// <summary>
        /// Получает или задаёт дату учёта продукции.
        /// </summary>
        /// <remarks>
        /// Указывает дату, когда был произведён учёт продукции.
        /// </remarks>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Получает или задаёт количество продукции.
        /// </summary>
        /// <remarks>
        /// Указывает количество единиц продукции, учтённых в записи.
        /// </remarks>
        public int Quantity { get; set; }

        /// <summary>
        /// Получает или задаёт дату последнего движения продукции.
        /// </summary>
        /// <remarks>
        /// Указывает дату последнего перемещения продукции. Может быть null, если движения не было.
        /// </remarks>
        public DateTime? LastMovementDate { get; set; }

        /// <summary>
        /// Получает или задаёт статус движения продукции.
        /// </summary>
        /// <remarks>
        /// Описывает текущее состояние продукции (например, "На складе", "В пути"). Может быть null.
        /// </remarks>
        public string MovementStatus { get; set; }
    }
}
