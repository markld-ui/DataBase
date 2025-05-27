using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Представляет модель поставки товаров в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс используется для хранения и передачи данных о поставках товаров,
    /// включая информацию о продукте, поставщике, дате поставки и количестве.
    /// </para>
    /// <para>
    /// Свойства класса соответствуют полям в базе данных и могут использоваться
    /// в операциях CRUD через соответствующие репозитории.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var supply = new Supply
    /// {
    ///     SupplyId = 1,
    ///     ProductId = 5,
    ///     SupplierId = 3,
    ///     SupplyDate = DateTime.Now,
    ///     Quantity = 100
    /// };
    /// Console.WriteLine($"Поставка #{supply.SupplyId}: {supply.Quantity} единиц товара");
    /// </code>
    /// </example>
    public class Supply
    {
        /// <summary>
        /// Получает или задает уникальный идентификатор поставки.
        /// </summary>
        /// <remarks>
        /// Первичный ключ для записи о поставке в базе данных.
        /// </remarks>
        public int SupplyId { get; set; }

        /// <summary>
        /// Получает или задает идентификатор продукта.
        /// </summary>
        /// <remarks>
        /// Внешний ключ, ссылается на <see cref="Product.ProductId"/>.
        /// </remarks>
        public int ProductId { get; set; }

        /// <summary>
        /// Получает или задает идентификатор поставщика.
        /// </summary>
        /// <remarks>
        /// Внешний ключ, ссылается на <see cref="Supplier.SupplierId"/>.
        /// </remarks>
        public int SupplierId { get; set; }

        /// <summary>
        /// Получает или задает дату поставки товара.
        /// </summary>
        public DateTime SupplyDate { get; set; }

        /// <summary>
        /// Получает или задает количество поставленного товара.
        /// </summary>
        public int Quantity { get; set; }
    }
}