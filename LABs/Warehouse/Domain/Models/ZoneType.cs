using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Определяет типы зон хранения на складе.
    /// </summary>
    /// <remarks>
    /// Перечисление используется для классификации различных зон хранения
    /// в системе управления складом.
    /// </remarks>
    public enum ZoneType
    {
        /// <summary>
        /// Холодильная зона для хранения продуктов, требующих охлаждения.
        /// </summary>
        refrigerated,

        /// <summary>
        /// Сухая зона для хранения продуктов, не требующих специальных условий.
        /// </summary>
        dry,

        /// <summary>
        /// Морозильная зона для хранения продуктов при отрицательных температурах.
        /// </summary>
        frozen,

        /// <summary>
        /// Общая зона хранения для различных типов продуктов.
        /// </summary>
        general
    }
}
