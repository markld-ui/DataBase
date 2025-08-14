namespace Domain.Models
{
    /// <summary>
    /// Представляет модель продукта в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс используется для хранения и передачи данных о продукте, включая его идентификатор,
    /// название, срок годности, тип продукта, статус активности и фотографию.
    /// </para>
    /// <para>
    /// Свойства класса соответствуют полям в базе данных и могут использоваться в операциях
    /// CRUD через репозитории.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var product = new Product
    /// {
    ///     ProductId = 1,
    ///     Name = "Молоко",
    ///     ExpiryDate = new DateTime(2025, 12, 31),
    ///     ProductType = ProductType.Food,
    ///     IsActive = true,
    ///     Photo = File.ReadAllBytes("milk_photo.jpg")
    /// };
    /// Console.WriteLine($"Продукт: {product.Name}, Срок годности: {product.ExpiryDate?.ToShortDateString() ?? "Не указан"}");
    /// </code>
    /// </example>
    public class Product
    {
        /// <summary>
        /// Получает или задаёт идентификатор продукта.
        /// </summary>
        /// <remarks>
        /// Уникальный идентификатор продукта в базе данных (первичный ключ).
        /// </remarks>
        public int ProductId { get; set; }

        /// <summary>
        /// Получает или задаёт название продукта.
        /// </summary>
        /// <remarks>
        /// Содержит название продукта (например, "Молоко"). Не должно быть null или пустым.
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Получает или задаёт срок годности продукта.
        /// </summary>
        /// <remarks>
        /// Указывает дату истечения срока годности продукта. Может быть null, если срок годности не применим.
        /// </remarks>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Получает или задаёт тип продукта.
        /// </summary>
        /// <remarks>
        /// Определяет категорию продукта, представленную перечислением <see cref="ProductType"/>.
        /// </remarks>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// Получает или задаёт статус активности продукта.
        /// </summary>
        /// <remarks>
        /// Указывает, активен ли продукт (true) или помечен как неактивный (false).
        /// </remarks>
        public bool IsActive { get; set; }

        /// <summary>
        /// Получает или задаёт фотографию продукта.
        /// </summary>
        /// <remarks>
        /// Хранит изображение продукта в виде массива байтов. Может быть null, если фотография отсутствует.
        /// </remarks>
        public byte[] Photo { get; set; }
    }
}
