namespace Domain.Models
{
    /// <summary>
    /// Представляет модель поставщика в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс используется для хранения и передачи данных о поставщике, включая его идентификатор,
    /// название компании, контактное лицо, телефон и адрес.
    /// </para>
    /// <para>
    /// Свойства класса соответствуют полям в базе данных и могут использоваться в операциях
    /// CRUD через репозитории.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var supplier = new Supplier
    /// {
    ///     SupplierId = 1,
    ///     CompanyName = "ООО Поставщик",
    ///     ContactPerson = "Иванов Иван",
    ///     Phone = "+7 (123) 456-78-90",
    ///     Address = "г. Москва, ул. Примерная, д. 1"
    /// };
    /// Console.WriteLine($"Поставщик: {supplier.CompanyName}, Контакт: {supplier.ContactPerson}");
    /// </code>
    /// </example>
    public class Supplier
    {
        /// <summary>
        /// Получает или задаёт идентификатор поставщика.
        /// </summary>
        /// <remarks>
        /// Уникальный идентификатор поставщика в базе данных (первичный ключ).
        /// </remarks>
        public int SupplierId { get; set; }

        /// <summary>
        /// Получает или задаёт название компании поставщика.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Получает или задаёт контактное лицо поставщика.
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Получает или задаёт контактный телефон поставщика.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Получает или задаёт адрес поставщика.
        /// </summary>
        public string Address { get; set; }
    }
}