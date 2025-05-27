using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Представляет модель сотрудника в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Класс используется для хранения и передачи данных о сотруднике, включая его идентификатор,
    /// фотографию, полное имя, должность и контактный телефон.
    /// </para>
    /// <para>
    /// Свойства класса соответствуют полям в базе данных и могут использоваться в операциях
    /// CRUD через репозитории.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var employee = new Employee
    /// {
    ///     EmployeeId = 1,
    ///     Photo = File.ReadAllBytes("employee_photo.jpg"),
    ///     FullName = "Иван Иванов",
    ///     Position = "Менеджер склада",
    ///     Phone = "+7 (123) 456-78-90"
    /// };
    /// Console.WriteLine($"Сотрудник: {employee.FullName}, Должность: {employee.Position}");
    /// </code>
    /// </example>
    public class Employee
    {
        /// <summary>
        /// Получает или задаёт идентификатор сотрудника.
        /// </summary>
        /// <remarks>
        /// Уникальный идентификатор сотрудника в базе данных (первичный ключ).
        /// </remarks>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Получает или задаёт фотографию сотрудника.
        /// </summary>
        /// <remarks>
        /// Хранит фотографию сотрудника в виде массива байтов. Может быть null, если фотография отсутствует.
        /// </remarks>
        public byte[] Photo { get; set; }

        /// <summary>
        /// Получает или задаёт полное имя сотрудника.
        /// </summary>
        /// <remarks>
        /// Содержит полное имя сотрудника (например, "Иван Иванов"). Не должно быть null или пустым.
        /// </remarks>
        public string FullName { get; set; }

        /// <summary>
        /// Получает или задаёт должность сотрудника.
        /// </summary>
        /// <remarks>
        /// Указывает должность сотрудника в организации (например, "Менеджер склада"). Не должно быть null или пустым.
        /// </remarks>
        public string Position { get; set; }

        /// <summary>
        /// Получает или задаёт контактный телефон сотрудника.
        /// </summary>
        /// <remarks>
        /// Содержит номер телефона сотрудника в текстовом формате (например, "+7 (123) 456-78-90").
        /// Может быть null, если телефон не указан.
        /// </remarks>
        public string Phone { get; set; }
    }
}
